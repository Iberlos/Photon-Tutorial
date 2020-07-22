﻿using UnityEngine;

namespace PUNBT
{
    /// <summary>
    /// Photon.PunBehaviour implements IPunCallbacks and deals with the struggle of having all the methods doing nothing. Inheriting from it allows us to override these methods, thus using only the ones we need.
    /// </summary>
    public class Launcher : Photon.PunBehaviour
    {
        #region Public Variables
        /// <summary>
        /// The PUN loglevel.
        /// </summary>
        public PhotonLogLevel LogLevel = PhotonLogLevel.Informational;
        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so, a new room will be created.")]
        public byte MaxPlayersPerRoom = 4;
        #endregion
        #region Private Variables
        /// <summary>
        /// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
        /// </summary>
        private string _gameVersion = "1";

        #endregion
        #region MonoBehaviour Callbacks
        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        private void Awake()
        {
            // #NotImportant
            // Force LogLevel
            PhotonNetwork.logLevel = LogLevel;

            // #Critical
            // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
            PhotonNetwork.autoJoinLobby = false;

            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.automaticallySyncScene = true;
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        private void Start()
        {
            Connect();
        }

        #endregion
        #region Public Methods
        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            // we check if we are connected or not, we join if we are, else we initiate the connection to the server.
            if(PhotonNetwork.connected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings(_gameVersion);
            }
        }
        #endregion
        #region Private Methods

        #endregion
        #region Photon.PunBehaviour Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("DemoAnimatior/Launcher: OnConnectedToMaster() was called by PUN");
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnPhotonRandomJoinFailed()
            PhotonNetwork.JoinRandomRoom();
        }
        public override void OnDisconnectedFromPhoton()
        {
            Debug.Log("DemoAnimatior/Launcher: OnDisconnectedFromPhoton() was called by PUN");
        }
        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("DemoAnimator/Launcher: OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. \nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }
        #endregion
    }
}

