using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pufferfish))]
public class PufferfishInspector : Editor
{
    private GUIStyle guiStyle = new GUIStyle();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("State", EditorStyles.boldLabel);
        Pufferfish pufferfish = (Pufferfish)target;
        if (pufferfish != null)
        {
            if (pufferfish.StateMachine.CurrentState != null)
            {
                System.Type state = pufferfish.StateMachine.CurrentState.GetType();
                GUILayout.Label(state.ToString());
            }
            else
            {
                GUILayout.Label("null");
            }
        }
        else
        {
            GUILayout.Label("Uninstantiated");
        }

    }
}