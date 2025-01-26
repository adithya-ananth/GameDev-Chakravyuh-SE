using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class WeightController : MonoBehaviour
{
    public static int logs;
    public Rigidbody2D body;

    [SerializeField] private TextMeshProUGUI logsText;

    private void Start()
    {
        logs = 0;
        logsText.text = "logs: 0";
    }

    public void AddWeight()
    {
        if (logs < 5)
        {
            logs++;
            PlayerController.modifyJumpForce();
        }

        logsText.text = $"logs: {logs}";
    }

    public void RemoveWeight()
    {
        if (logs > 0)
        {
            logs--;
            PlayerController.modifyJumpForce();
        }
        logsText.text = $"logs: {logs}";
    }

}
