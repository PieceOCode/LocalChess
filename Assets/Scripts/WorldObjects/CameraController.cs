using UnityEngine;

namespace Chess
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private BoardRepresentation board;

        private Camera cam = default;

        void Start()
        {
            cam = GetComponent<Camera>();
        }

        void Update()
        {
            // Position (0,0) marks the upper left corner of the board, therefore the camera has to be moved to the boards center.
            cam.transform.position = new Vector3(board.Width / 2 - 0.5f, board.Height / 2 - 0.5f, cam.transform.position.z);


            // Calculate the size of the camera depending on the screen aspect
            // When screen is higher than wide, the camera should scale higher
            cam.orthographicSize = Mathf.Max(board.Width / 2, (board.Width / 2) / cam.aspect);

            // If the screen is in portrait mode (e.g. mobile) then move the chess field slightly up, because the UI is placed differently.
            //if (Screen.width < Screen.height)
            //{
            //    Vector3 screenCorner = cam.ScreenToWorldPoint(new Vector3(0, 0.1f * (float)Screen.height, 0));
            //    cam.transform.position = new Vector3(cam.transform.position.x, screenCorner.y + board.Height / 2, cam.transform.position.z);
            //}
        }
    }
}