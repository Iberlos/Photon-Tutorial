using UnityEngine;
using UnityEngine.SceneManagement;


namespace PUNBT
{
    public class GameManager : MonoBehaviour
    {
        #region Photon Messages
        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public void OnLeftRoom()
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
    }
}