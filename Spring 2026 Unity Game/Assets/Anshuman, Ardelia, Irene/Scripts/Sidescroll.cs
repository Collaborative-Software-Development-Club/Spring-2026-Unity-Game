using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sidescroll : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camera;

    private string wpath;
    private string spath;
    private int windex;
    private int sindex;

    void Start() {
        findKey("w", out wpath, out windex);
        findKey("s", out spath, out sindex);
        // toggleKey(Globals.wKeyPath, Globals.wKeyIndex);
        // toggleKey(Globals.sKeyPath, Globals.sKeyIndex);
        inputReader.Actions.Player.Move.ApplyBindingOverride(windex, "");
        inputReader.Actions.Player.Move.ApplyBindingOverride(sindex, "");
        
        Vector3 playerPos = player.transform.position;
        camera.transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z - 5);
        camera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void findKey(string key, out string path, out int index) {
        var bindings = inputReader.Actions.Player.Move.bindings;
        string str = "";
        int ind = -1;
        for (int i = 0; i < bindings.Count; i++) {
            if (bindings[i].path.Contains("<Keyboard>/" + key)) {
                ind = i;
                str = bindings[i].path;
                break;
            }
        }

        index = ind;
        path = str;
    }

    private void toggleKey(string path, int index) {
        var moveAction = inputReader.Actions.Player.Move;

        if (index != -1) {
            if (!string.IsNullOrEmpty(moveAction.bindings[index].overridePath)) {
                moveAction.ApplyBindingOverride(index, path);
            } else {
                moveAction.ApplyBindingOverride(index, "");
            }
        }
    }
}
