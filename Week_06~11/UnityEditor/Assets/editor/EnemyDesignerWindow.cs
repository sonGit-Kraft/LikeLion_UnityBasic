using UnityEngine;
using UnityEditor;

public class EnemyDesignerWindow : EditorWindow
{
    [MenuItem("Window/Enemy Designer")]

    static void OpenWindow()
    {
        EnemyDesignerWindow window = (EnemyDesignerWindow)GetWindow(typeof(EnemyDesignerWindow));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    int countX = 0;
    int countZ = 0;
    // 라벨
    private void OnGUI()
    {
        GUILayout.Label("적에 관한 내용을 만드는 툴", EditorStyles.boldLabel);
        GUILayout.Label("이건 그냥 라벨");
        EditorGUILayout.LabelField("라벨 필드: ", "보스");

        GUILayout.Label("큐브 만들기", EditorStyles.boldLabel);

        if(GUILayout.Button("Create CubeX"))
        {
            countZ = 0;
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "MapX";
            cube.transform.position = new Vector3(countX++, 0, countZ);
        }
        if (GUILayout.Button("Create CubeZ"))
        {
            countX = 0;
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "MapZ";
            cube.transform.position = new Vector3(countX, 0, countZ++);
        }
    }
}
