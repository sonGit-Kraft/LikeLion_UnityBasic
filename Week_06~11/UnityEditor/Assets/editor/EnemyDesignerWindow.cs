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
    // ��
    private void OnGUI()
    {
        GUILayout.Label("���� ���� ������ ����� ��", EditorStyles.boldLabel);
        GUILayout.Label("�̰� �׳� ��");
        EditorGUILayout.LabelField("�� �ʵ�: ", "����");

        GUILayout.Label("ť�� �����", EditorStyles.boldLabel);

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
