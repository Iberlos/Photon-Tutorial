using UnityEngine;
using UnityEngine.SceneManagement;


namespace PUNBT
{
    public class GameManager : Photon.PunBehaviour
    {
        #region Photon Messages
        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }
        #endregion
        #region Public Methods
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion
        #region Private Methods
        private void LoadArena()
        {
            if(!PhotonNetwork.isMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to load a level but we are not the master client");
            }
            Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.room.PlayerCount);
        }
        #endregion
        #region Photon Messages
        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            Debug.Log("OnPhotonPlayerConnected() " + newPlayer.NickName); // not seen if you're the player connecting

            if(PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected
                LoadArena();
            }
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            Debug.Log("OnPhotonPlayerDisconected() " + otherPlayer.NickName); // seen when other disconnects
            if(PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerDisconnected isMasterClient " + PhotonNetwork.isMasterClient);// called before OnPhotonPlayerDisconnected
                LoadArena();
            }
        }
        #endregion
    }
}