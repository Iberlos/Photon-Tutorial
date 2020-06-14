/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject controlPanel;

        [SerializeField]
        private Text feedbackText;

        [SerializeField]
        private byte maxPlayersPerRoom = 2;

        bool isConnecting;

        string gameVersion = "1";

        [Space(10)]
        [Header("Custom Variables")]
        public InputField playerNameField;
        public InputField roomNameField;

        [Space(5)]
        public Text playerStatus;
        public Text connectionStatus;

        [Space(5)]
        public GameObject roomJoinUI;
        public GameObject buttonLoadArena;
        public GameObject buttonJoinRoom;

        string playerName = "";
        string roomName = "";

        // Start Method
        private void Start()
        {
            /*1.When connecting to a server, PUN pings all available servers and stores the IP address of the server with the lowest ping as a PlayerPrefs key-value pair. 
            This can lead to unexpected behavior during the connection stage. To avoid any anomalies, DeleteAll is called when the Launcher scene starts.*/
            PlayerPrefs.DeleteAll();
            Debug.Log("Connecting to Photon Network...");

            /*2.The UI elements are hidden by default, and are activated once a connection to a Photon server is established.*/
            roomJoinUI.SetActive(false);
            buttonLoadArena.SetActive(false);

            /*3. ConnectToPhoton is called to connect to the Photon network.*/
            ConnectToPhoton();

        }

        private void Awake()
        {
            /*4. The value of AutomaticallySyncScene is set to true. This is used to sync the scene across all the connected players in a room.*/
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Helper Methods
        public void SetPlayerName(string name)
        {
            playerName = name;
        }

        public void SetRoomName(string name)
        {
            roomName = name;
        }

        // Tutorial Methods
        void ConnectToPhoton()
        {
            connectionStatus.text = "Connecting...";
            /*1. The GameVersion parameter is set. This is the version string for your build and can be used to separate incompatible clients. 
             * For this tutorial, it will be set to 1 (set when the gameVersion field is declared).*/
            PhotonNetwork.GameVersion = gameVersion;
            /*2. ConnectUsingSettings is called, which is used to connect to Photon as configured in the editor. You can read more in the docs.*/
            PhotonNetwork.ConnectUsingSettings();
        }

        public void JoinRoom()
        {
            if(PhotonNetwork.IsConnected)
            {
                /*1. The NickName parameter of the LocalPlayer is set from the private variable playerName. 
                 This is the name that will be available to everyone you play with on the Photon network, and is used as a unique identifier.*/
                PhotonNetwork.LocalPlayer.NickName = playerName;
                Debug.Log("PhotonNetwork.IsConnected | Trying to Create/Join Roon" + roomNameField.text);
                /*2. An object of class RoomOptions is declared. This wraps up common room properties required when you create a room. 
                 It can be used to give the user control of various characteristics of the room such as maximum number of players that can join, 
                 PlayerTtl (Player Time To Live), etc. (Docs)*/
                RoomOptions roomOptions = new RoomOptions();
                /*3. An object of class TypedLobby is declared. This refers to a specific lobby type on the Photon server. The name and lobby type are used as an unique identifier. 
                 The Room name is set from the private variable roomName and the Lobby type is set as Default. (Docs)*/
                TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default);
                /*4. Finally, the JoinOrCreateRoom method of PhotonNetwork class is called with arguments — roomName, roomOptions and typedLobby that were set earlier.
                 If the method is called by a new user with a new Room name that does not yet exist, a room is created and the user is set as the Lobby Leader. 
                 Otherwise, the other players just join the room.*/
                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
            }
        }

        public void LoadArena()
        {
            /*5. Once the Lobby Leader has created and joined a Room, the LoadArena button will be set active. 
             A check is set before loading the Arena to make sure the MainArena scene is loaded only if both players have joined the room.*/
             if(PhotonNetwork.CurrentRoom.PlayerCount > 1)
            {
                PhotonNetwork.LoadLevel("MainArena");
            }
            else
            {
                playerStatus.text = "Minimum 2 players required to Load Arena";
            }
        }

        // Photon Methods
        public override void OnConnected()
        {
            /*1. As the name suggests, OnConnected gets invoked when the user connects to the Photon Network. Here, the method calls the base method onConnected(). 
             Any additional code that needs to be executed is written following this method call.*/
            base.OnConnected();
            /*2. These methods provide feedback to the user. When the user successfully connects to the Photon Network, the UI Text connectionStatus is set, 
             and the roomJoinUI GameObject is set to visible.*/
            connectionStatus.text = "Connected to Photon!";
            connectionStatus.color = Color.green;
            roomJoinUI.SetActive(true);
            buttonLoadArena.SetActive(false);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            /*3. OnDisconnected gets called if the user gets Disconnected from the Photon Network. In this case, the controlPanel GameObject is set to false,
             and an Error type message is logged to Unity.*/
            isConnecting = false;
            controlPanel.SetActive(true);
            Debug.LogError("Disconnected. Please check your internet connection.");
        }

        public override void OnJoinedRoom()
        {
            /*4. Finally, OnJoinedRoom is called when the user joins a room. Here, it checks if the user is the Master Client (the first user to join the room). 
             If so, the user is set as the lobby leader and is shown a message to indicate this. The lobby leader has the power to load the MainArena scene, 
             which is a common way of creating a room in most popular multiplayer games. Otherwise, if the user is not first to join the room, 
             a message is shown to tell that user that they’ve successfully joined the room.*/

            if(PhotonNetwork.IsMasterClient)
            {
                buttonLoadArena.SetActive(true);
                buttonJoinRoom.SetActive(false);
                playerStatus.text = "You are Lobby Leader";
            }
            else
            {
                playerStatus.text = "Connected to Lobby";
            }
        }
    }
}
