using UnityEngine;
using UnityEngine.SceneManagement; // To reload the scene

public class TreeInteractionManager : MonoBehaviour
{
    public GameObject[] trees; // Array of tree objects
    public GameObject scrollPanel; // Reference to the Scroll panel

    private string selectedTool = ""; // Tracks the currently selected tool
    private int[] treeTapCounts; // Tracks tap counts for each tree
    private int remainingTrees; // Number of trees remaining

    void Start()
    {
        // Initialize tap counts for each tree
        treeTapCounts = new int[trees.Length];
        remainingTrees = trees.Length; // Set the initial number of trees

        // Ensure the scroll panel is hidden at the start
        if (scrollPanel != null)
        {
            scrollPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Scroll Panel is not assigned in the Inspector!");
        }
    }

    public void SelectTool(string tool)
    {
        selectedTool = tool;
        Debug.Log($"Selected Tool: {tool}");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect mouse click
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedTree = hit.collider.gameObject;

                // Check if the clicked object is a tree
                for (int i = 0; i < trees.Length; i++)
                {
                    if (trees[i] == clickedTree)
                    {
                        HandleTreeInteraction(i, clickedTree);
                        break;
                    }
                }
            }
        }
    }

    void HandleTreeInteraction(int treeIndex, GameObject tree)
    {
        // Play the tree's AudioSource sound
        AudioSource treeAudio = tree.GetComponent<AudioSource>();
        if (treeAudio != null)
        {
            treeAudio.Play(); // Play the attached audio clip
        }
        else
        {
            Debug.LogWarning($"No AudioSource found on tree: {tree.name}");
        }

        treeTapCounts[treeIndex]++;
        Debug.Log($"Tree {tree.name} tap count: {treeTapCounts[treeIndex]}");

        if (treeTapCounts[treeIndex] >= 3)
        {
            Destroy(tree);
            remainingTrees--;

            if (selectedTool == "Axe")
            {
                Debug.Log("Tree cut down.");
            }
            else if (selectedTool == "Fire")
            {
                Debug.Log("Deforestation and forest fire have increased global warming leading to no sustainability.");
            }

            if (remainingTrees == 0)
            {
                PointsManager.IncrementPoints(-10);
                SceneManager.LoadScene("Map2");
            }
        }
    }

  
}
