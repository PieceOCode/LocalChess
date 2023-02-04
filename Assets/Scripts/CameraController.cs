using UnityEngine;

namespace Chess
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Board board;

        private Camera camera = default;

        // Start is called before the first frame update
        void Start()
        {
            camera = GetComponent<Camera>();
            // Set camera to the middle of the board
            camera.transform.position = new Vector3(board.Width / 2 - 0.5f, board.Height / 2 - 0.5f, camera.transform.position.z);

            // Calculate the size of the camera depending on the screen aspect
            // When screen is higher than wide, the camera should scale higher
            camera.orthographicSize = Mathf.Max(board.Width / 2, (board.Width / 2) / camera.aspect);
        }

        // Update is called once per frame
        void Update()
        {
            camera.orthographicSize = Mathf.Max(board.Width / 2, (board.Width / 2) / camera.aspect);
        }
    }
}