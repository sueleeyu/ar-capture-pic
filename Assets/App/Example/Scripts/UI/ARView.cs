using UnityEngine;
using UnityEngine.UI;

namespace FrameworkDesign.Example
{

    public class ARView : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            //Logger.Log("HomePage-Start");
            transform.Find("SafeArea/BackButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                Logger.Log("ARView-onClick");
                gameObject.SetActive(false);
                new GameCloseCommand().Excute();
            }

            );
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
