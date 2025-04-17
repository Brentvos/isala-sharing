using New;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotebookTaskManager : MonoBehaviour
{
    public static NotebookTaskManager Instance;

    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI pageIndicator_TXT;
    [SerializeField] private GameObject warningToDoInGame;
    [SerializeField] private GameObject warningToDoInNotebook;


    private List<PatientTaskElement> elements = new List<PatientTaskElement>();

    private int curPage = 1;
    private int totalPages;

    private void Awake()
    {
        if (Instance != this)
            Instance = this;

        previousButton.onClick.AddListener(() => OpenPreviousPage());
        nextButton.onClick.AddListener(() => OpenNextPage());

        totalPages = Mathf.Max(1, Mathf.CeilToInt((float)elements.Count / 10));
        pageIndicator_TXT.text = curPage.ToString() + "/" + totalPages.ToString();

        pageIndicator_TXT.gameObject.SetActive(false);
        previousButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    public void Add(PatientTaskElement elem)
    {
        elements.Add(elem);
        Sort();

        totalPages = Mathf.Max(1, Mathf.CeilToInt((float)elements.Count / 10));
        Refresh();
    }

    public void Remove(PatientTaskElement elem)
    {
        elements.Remove(elem);
        Destroy(elem.gameObject);

        Sort();

        totalPages = Mathf.Max(1, Mathf.CeilToInt((float)elements.Count / 10));
        Refresh();
    }

    public void Sort()
    {
        int amountToDo = 0;

        var sortedElements =
            elements.OrderByDescending(x => (int) (x.GetTreatmentState))
            .Reverse().ToList();

        elements = sortedElements;

        foreach(PatientTaskElement element in elements)
        {
            element.transform.SetSiblingIndex(
                elements.IndexOf(element));

            if (element.GetTreatmentState == TREATMENT_STATE.TO_DO)
            {
                amountToDo++;
            }
        }

        if (amountToDo > 0)
        {
            warningToDoInGame.SetActive(true);
            warningToDoInNotebook.SetActive(true);

        }

        else
        {
            warningToDoInGame.SetActive(false);
            warningToDoInNotebook.SetActive(false);

        }
    }

    private void Refresh()
    {
        int startIndex = (curPage - 1) * 10;
        int endIndex = Mathf.Min(startIndex + 10, elements.Count);

        // Loop through all elements and toggle their active state
        for (int i = 0; i < elements.Count; i++)
        {
            if (i >= startIndex && i < endIndex)
            {
                elements[i].gameObject.SetActive(true); // Show elements on the current page
            }
            else
            {
                elements[i].gameObject.SetActive(false); // Hide elements not on the current page
            }
        }

        pageIndicator_TXT.text = curPage.ToString() + "/" + totalPages.ToString();

        if (totalPages <= 1)
        {
            pageIndicator_TXT.gameObject.SetActive(false);
            previousButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
        }

        else
        {
            pageIndicator_TXT.gameObject.SetActive(true);
            previousButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
    }

    private void OpenNextPage()
    {
        if (curPage < totalPages)
            curPage++;

        else
            curPage = 1;

        Refresh();
    }

    private void OpenPreviousPage()
    {
        if (curPage > 1)
            curPage--;

        else
            curPage = totalPages;

        Refresh();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale *= 2;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale *= 0.5f;
        }
    }
}
