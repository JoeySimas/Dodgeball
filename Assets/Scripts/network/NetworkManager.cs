using System; //for exception
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private SocketIO.SocketIOComponent io;
    private double timePassed = 0.0;
    public GameObject playerPrefab;
    public dodgeballManager dodgeballManager;
    public class player
    {
        public string id { get; set; }
        public Transform transform { get; set; }
        public Vector3 newPos { get; set; }
        public Quaternion newRot { get; set; }
        public networkPlayerController network;

    }
    List<player> otherPlayers = new List<player>();

    void Start()
    {
        io = GetComponent<SocketIO.SocketIOComponent>();
        dodgeballManager = transform.parent.GetComponentInChildren<dodgeballManager>();

        io.On("joined-game", (SocketIO.SocketIOEvent e) =>
        {
            JSONObject data = e.data;
            Debug.Log("Joined game " + data);
        });
        io.On("player-position-update", (SocketIO.SocketIOEvent e) =>
        {

            try
            {
                JSONObject data = e.data;
                string playerID = data.GetField("id").ToString();

                JSONObject playerPos = data.GetField("position");
                Vector3 newPos = new Vector3(
                    float.Parse(playerPos.GetField("x").ToString()),
                    float.Parse(playerPos.GetField("y").ToString()),
                    float.Parse(playerPos.GetField("z").ToString())
                );

                JSONObject playerRot = data.GetField("rotation");
                Quaternion newRot = new Quaternion(
                    float.Parse(playerRot.GetField("x").ToString()),
                    float.Parse(playerRot.GetField("y").ToString()),
                    float.Parse(playerRot.GetField("z").ToString()),
                    float.Parse(playerRot.GetField("w").ToString())
                );

                player p = getPlayer(playerID);
                if (p != null)
                {
                    p.newPos = newPos;
                    p.newRot = newRot;
                }
            }
            catch (Exception err)
            {
                Debug.LogError("player-position-update-exception: " + err);
            }
        });
        io.On("dodgeball-thrown", (SocketIO.SocketIOEvent e) =>
        {
            JSONObject data = e.data;
            JSONObject posObj = data.GetField("position");
            JSONObject dirObj = data.GetField("direction");
            int ID = int.Parse(data.GetField("ID").ToString());
            string playerID = data.GetField("playerID").ToString();

            Vector3 position = new Vector3(
                 float.Parse(posObj.GetField("x").ToString()),
                 float.Parse(posObj.GetField("y").ToString()),
                 float.Parse(posObj.GetField("z").ToString())
                );
            Vector3 direction = new Vector3(
                float.Parse(dirObj.GetField("x").ToString()),
                 float.Parse(dirObj.GetField("y").ToString()),
                 float.Parse(dirObj.GetField("z").ToString())
            );
            dodgeballManager.onNetworkThrowBall(position, direction);
            getPlayer(playerID).network.onThrowDodgeball();
        });
        io.On("dodgeball-pickup", (SocketIO.SocketIOEvent e) =>
        {
            JSONObject data = e.data;
            string playerID = data.GetField("playerID").ToString();

            player foundPlayer = getPlayer(playerID);
            foundPlayer.network.onPickupDodgeball();
        });
        io.On("player-join", (SocketIO.SocketIOEvent e) =>
        {
            Debug.Log("new player join");
            JSONObject data = e.data;

            player newPlayer = new player();
            newPlayer.id = data.GetField("playerID").ToString();
            GameObject playerObj = GameObject.Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            newPlayer.transform = playerObj.transform;
            newPlayer.network = playerObj.GetComponent<networkPlayerController>();
            otherPlayers.Add(newPlayer);
        });
        io.On("player-disconnect", (SocketIO.SocketIOEvent e) =>
        {
            JSONObject data = e.data;
            string id = data.GetField("id").ToString();
            player p = otherPlayers.Find(x => x.id == id);
            GameObject.Destroy(p.transform.gameObject);
            otherPlayers.Remove(p);
        });
        io.On("player-death", (SocketIO.SocketIOEvent e) =>
        {
            Debug.Log("player death");
        });
    }
    void FixedUpdate()
    {
        UpdateOtherPlayerPositions();
    }
    void UpdateOtherPlayerPositions()
    {
        Vector3 pos;
        Quaternion rot;
        foreach (player p in otherPlayers)
        {
            if (!p.transform.position.Equals(p.newPos))
            {
                pos = Vector3.Lerp(p.transform.position, p.newPos, Time.deltaTime * 15);
                Vector3 dist = p.transform.position - p.newPos;

                p.transform.position = pos;
            }
            if (!p.transform.rotation.Equals(p.newRot))
            {
                rot = Quaternion.Lerp(p.transform.rotation, p.newRot, Time.deltaTime * 30);
                p.transform.rotation = rot;
            }
        }
    }
    public void sendTransform(Vector3 pos, Quaternion rot)
    {
        if (io.IsConnected)
        {
            JSONObject data = new JSONObject();
            JSONObject position = new JSONObject();
            position.AddField("x", pos.x);
            position.AddField("y", pos.y);
            position.AddField("z", pos.z);

            JSONObject rotation = new JSONObject();
            rotation.AddField("x", rot.x);
            rotation.AddField("y", rot.y);
            rotation.AddField("z", rot.z);
            rotation.AddField("w", rot.w);

            data.AddField("position", position);
            data.AddField("rotation", rotation);

            io.Emit("player-position", data);
        }

    }
    public void ThrowBall(int ID, Vector3 position, Vector3 direction)
    {
        JSONObject data = new JSONObject();
        JSONObject pos = new JSONObject();
        JSONObject dir = new JSONObject();

        pos.AddField("x", position.x);
        pos.AddField("y", position.y);
        pos.AddField("z", position.z);

        dir.AddField("x", direction.x);
        dir.AddField("y", direction.y);
        dir.AddField("z", direction.z);

        data.AddField("ID", ID);
        data.AddField("position", pos);
        data.AddField("direction", dir);

        io.Emit("throw-dodgeball", data);
    }
    public void PickupBall()
    {
        Debug.Log("commit acciont pickup ball");
        io.Emit("pickup-dodgeball");
    }
    player getPlayer(string id)
    {
        return otherPlayers.Find(player => player.id == id);
    }
}
