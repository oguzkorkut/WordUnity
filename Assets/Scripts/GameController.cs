using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WordShufflerConsoleTest;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static LetterClass[] LetterButton = new LetterClass[16];
    public LetterClass LetterPrefab;
    public static string s_enteredWord = "";
    public static ArrayList s_enteredWord_position = new ArrayList();


    //Letters and Words Variables
    public LettersToType o_lettersToType = new LettersToType();
    private string key;

    //public static string[] 			templetters=new string[]{"n","c","y","n","s","e","p","t","s","o","r","d","h","l","h","e"};
    public static string[] templetters = new string[] { "O", "Y", "", "", "A", "Ğ", "U", "", "Ş", "A", "Z", "", "", "R", "", "" };
    public static System.Collections.Generic.List<string> definedlist;


    public static ArrayList checkwords = new ArrayList();
    public static ArrayList checkwordsenemy = new ArrayList();

    private int[] lettersgridx = new int[16];
    private int[] lettersgridy = new int[16];
    private int[] lettersgrid = new int[2];

    public static char[,] lettersmatrix;
    private bool tutorialwordblock;

    public GameObject LettersPanel;

    private bool IsSwipeTyping;
    private Vector3 SwipeStartPosition;
    private Vector3 SwipeCurrentPosition;

    public Camera GUICamera;
    public Text WordField;
    public Text PointsField;

    private float points;
    public float pointsmultipler;

    private int previouslettersid;
    private float PowerDemandGlobal;

    public class LetterToType
    {
        public string s_letter;
        public bool b_enabled;

        public LetterToType(string letter)
        {
            s_letter = letter;
            b_enabled = true;
        }
    }

    public class LettersToType
    {
        public List<LetterToType> l_letters = new List<LetterToType>();

        public void ReEnableAll()
        {
            for (int i = 0; i < l_letters.Count; i++)
            {
                if (!l_letters[i].b_enabled)
                {
                    l_letters[i].b_enabled = true;
                    LetterButton[i].GetComponent<LetterClass>().DisableConnect();
                }
            }
        }

        public void CorrectAll()
        {
            for (int i = 0; i < l_letters.Count; i++)
            {
                if (!l_letters[i].b_enabled)
                {
                    l_letters[i].b_enabled = true;
                    LetterButton[i].GetComponent<Animator>().SetTrigger("ReleaseCorrect");
                    LetterButton[i].GetComponent<LetterClass>().RightEffect();
                    LetterButton[i].GetComponent<LetterClass>().DisableConnect();
                    LetterButton[i].GetComponent<LetterClass>().DisableLight();
                    
                    LetterButton[i].GetComponent<LetterClass>().gameObject.GetComponent<SphereCollider>().enabled = false;
                    LetterButton[i].GetComponent<LetterClass>().LetterLabel.text = "";
                }
            }
        }

        public void IncorrectAll()
        {
            for (int i = 0; i < l_letters.Count; i++)
            {
                if (!l_letters[i].b_enabled)
                {
                    l_letters[i].b_enabled = true;
                    LetterButton[i].GetComponent<Animator>().SetTrigger("ReleaseWrong");
                    LetterButton[i].GetComponent<LetterClass>().WrongEffect();
                    LetterButton[i].GetComponent<LetterClass>().DisableConnect();
                }
            }
        }
    }

    //Reenable Buttons
    public void ReEnableAll2()
    {
        for (int i = 0; i < o_lettersToType.l_letters.Count; i++)
        {
            o_lettersToType.l_letters[i].b_enabled = true;
        }
    }

    public void InstantiateLetters()
    {
        // Clear previous letters
        o_lettersToType.l_letters.Clear();

        // Fill in rest of the letter list with random letters, then randomize list
        int i_temp = o_lettersToType.l_letters.Count;
        for (int i = 0; i < 16 - i_temp; i++)
        {
            Debug.Log("---" + templetters[i]);
            o_lettersToType.l_letters.Add(new LetterToType(templetters[i]));
        }

        s_enteredWord = "";
        s_enteredWord_position.Clear();

        LettersPanel.SetActive(true);

        int templettery = -1;

        Debug.Log(o_lettersToType.l_letters.Count);

        int count = 0;

        for (int i = 0; i < o_lettersToType.l_letters.Count; i++)
        {
            count++;
            Debug.Log(o_lettersToType.l_letters[i].s_letter);
            if (LetterButton[i] == null)
            {
                LetterButton[i] = Instantiate(LetterPrefab) as LetterClass;
            }          

            o_lettersToType.l_letters[i].b_enabled = true;
            LetterButton[i].transform.SetParent(LettersPanel.transform);
            LetterButton[i].transform.localScale = new Vector3(1, 1, 1);
            LetterButton[i].LetterLabel.text = o_lettersToType.l_letters[i].s_letter.ToUpper();
            LetterButton[i].Buttonid = i;

            lettersgridx[i] = i % 4;
            if (lettersgridx[i] == 0)
            {
                templettery += 1;
            }
                
            lettersgridy[i] = templettery % 4;

           // LetterButton[i].connectobject = null;

            if (o_lettersToType.l_letters[i].s_letter.Equals(""))
            {
                Debug.Log("Boş");
                //LetterButton[i].GetComponent<LetterClass>().gameObject.SetActive(false);
                LetterButton[i].GetComponent<LetterClass>().gameObject.GetComponent<SphereCollider>().enabled = false;

                //LetterButton[i].GetComponent<LetterClass>().gameObject.GetComponent<Image>().enabled = false;
                //LetterButton[i].GetComponent<LetterClass>().gameObject.SetActive(false);
                //LetterButton[i].GetComponent<LetterClass>().renderer.gameObject.enabled = false;
                //LetterButton[i].GetComponent<Renderer>().enabled = false;

                // Destroy(LetterButton[i].GetComponent<LetterClass>().gameObject);
                // LetterButton[i].connectobject.SetActive(false);
            }
        }

        Debug.Log("Count:" + count);
        IsSwipeTyping = false;
        LettersPanel.SetActive(true);

        o_lettersToType.ReEnableAll();
        ReEnableAll2();
    }

    // Use this for initialization
    void Start()
    {
        points = 0;

        WordField.text = "";
        PointsField.text = points.ToString();

        lettersgrid[0] = 0;
        lettersgrid[1] = 0;

        checkwords.Clear();

        IsSwipeTyping = false;

        CheckWord("");
        checkwords.Add("");

//        RandomSet.GetData();

        InstantiateLetters();
    }

    void Update()
    {

        RaycastHit hit = new RaycastHit();
        ControlTouchInput(hit);

    }

    public void ControlTouchInput(RaycastHit hit)
    {
        if (Input.GetKeyDown("mouse 0"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "LetterButtonTag")
                {
                    if (IsSwipeTyping == false)
                    {
                        int tempid = hit.transform.gameObject.GetComponent<LetterClass>().Buttonid;
                        if (o_lettersToType.l_letters[tempid].b_enabled == true)
                        {
                            previouslettersid = tempid;
                            hit.transform.gameObject.GetComponent<LetterClass>().LetterButtonPressed();
                            WordField.text = s_enteredWord;
                            IsSwipeTyping = true;
                            lettersgrid[0] = lettersgridx[tempid];
                            lettersgrid[1] = lettersgridy[tempid];
                            LetterButton[tempid].GetComponent<Animator>().SetTrigger("TouchEnter");// .Play("Pressed");
                        }
                    }
                }
            }
        }

        else if (Input.GetKey("mouse 0"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "LetterButtonTag")
                {
                    if (IsSwipeTyping == true)
                    {
                        int tempid = hit.transform.gameObject.GetComponent<LetterClass>().Buttonid;
                        if (o_lettersToType.l_letters[tempid].b_enabled == true)
                        {
                            if ((Mathf.Abs(lettersgridx[tempid] - lettersgrid[0]) <= 1) && (Mathf.Abs(lettersgridy[tempid] - lettersgrid[1]) <= 1))
                            {
                                hit.transform.gameObject.GetComponent<LetterClass>().LetterButtonPressed();
                                WordField.text = s_enteredWord;
                                lettersgrid[0] = lettersgridx[tempid];
                                lettersgrid[1] = lettersgridy[tempid];
                                LetterButton[tempid].gameObject.GetComponent<Animator>().SetTrigger("TouchEnter");// .Play("Pressed");

                                LetterButton[tempid].gameObject.GetComponent<LetterClass>().ConnectEffect(LetterButton[previouslettersid].transform.position, LetterButton[tempid].transform.position);

                                LetterButton[previouslettersid].gameObject.GetComponent<Animator>().SetTrigger("TouchLeave");// .Play("Pressed");
                                previouslettersid = tempid;
                            }
                        }
                    }
                }
            }
        }

        else if (Input.GetKeyUp("mouse 0"))
        {
            IsSwipeTyping = false;
            if (s_enteredWord.Length > 0)
            {
                if (CheckWord(s_enteredWord.ToLower()))
                {
                  //  LetterButton[i].GetComponent<LetterClass>().gameObject.GetComponent<SphereCollider>().enabled = false;
                    if (!checkwords.Contains(s_enteredWord.ToUpper()))
                    {
                        o_lettersToType.CorrectAll();

                        PowerDemandGlobal = (float)s_enteredWord.Length;
                        UpdateScore(PowerDemandGlobal);

                        checkwords.Add(s_enteredWord.ToUpper());
                    }
                    else
                    {
                        o_lettersToType.IncorrectAll();
                    }
                }
                else
                {
                    o_lettersToType.IncorrectAll();
                }

                s_enteredWord = "";
                WordField.text = s_enteredWord;
            }
        }

        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
            {
                Ray ray = GUICamera.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "LetterButtonTag")
                    {
                        if (IsSwipeTyping == false)
                        {
                            int tempid = hit.transform.gameObject.GetComponent<LetterClass>().Buttonid;
                            if (o_lettersToType.l_letters[tempid].b_enabled == true)
                            {
                                previouslettersid = tempid;
                                hit.transform.gameObject.GetComponent<LetterClass>().LetterButtonPressed();
                                WordField.text = s_enteredWord;
                                IsSwipeTyping = true;
                                lettersgrid[0] = lettersgridx[tempid];
                                lettersgrid[1] = lettersgridy[tempid];
                                LetterButton[tempid].GetComponent<Animator>().SetTrigger("TouchEnter");// .Play("Pressed");
                            }
                        }
                    }
                }
            }

            if (Input.GetTouch(i).phase.Equals(TouchPhase.Moved))
            {
                Ray ray = GUICamera.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "LetterButtonTag")
                    {
                        if (IsSwipeTyping == true)
                        {
                            int tempid = hit.transform.gameObject.GetComponent<LetterClass>().Buttonid;
                            if (o_lettersToType.l_letters[tempid].b_enabled == true)
                            {
                                if ((Mathf.Abs(lettersgridx[tempid] - lettersgrid[0]) <= 1) && (Mathf.Abs(lettersgridy[tempid] - lettersgrid[1]) <= 1))
                                {
                                    hit.transform.gameObject.GetComponent<LetterClass>().LetterButtonPressed();
                                    WordField.text = s_enteredWord;
                                    lettersgrid[0] = lettersgridx[tempid];
                                    lettersgrid[1] = lettersgridy[tempid];
                                    LetterButton[tempid].gameObject.GetComponent<Animator>().SetTrigger("TouchEnter");// .Play("Pressed");

                                    LetterButton[tempid].gameObject.GetComponent<LetterClass>().ConnectEffect(LetterButton[previouslettersid].transform.position, LetterButton[tempid].transform.position);

                                    LetterButton[previouslettersid].gameObject.GetComponent<Animator>().SetTrigger("TouchLeave");// .Play("Pressed");
                                    previouslettersid = tempid;
                                }
                            }
                        }
                    }
                }
            }

            if (Input.GetTouch(i).phase.Equals(TouchPhase.Ended))
            {
                IsSwipeTyping = false;
                if (s_enteredWord.Length > 0)
                {
                    if (CheckWord(s_enteredWord.ToLower()))
                    {
                        if (!checkwords.Contains(s_enteredWord.ToUpper()))
                        {
                            o_lettersToType.CorrectAll();

                            PowerDemandGlobal = (float)s_enteredWord.Length;
                            UpdateScore(PowerDemandGlobal);

                            checkwords.Add(s_enteredWord.ToUpper());
                        }
                        else
                        {
                            o_lettersToType.IncorrectAll();
                        }
                    }
                    else
                    {
                        o_lettersToType.IncorrectAll();
                    }

                    s_enteredWord = "";
                    WordField.text = s_enteredWord;
                }
            }
        }
    }

    public void UpdateScore(float pointstoadd)
    {
        points += pointstoadd * pointsmultipler;
        PointsField.text = points.ToString();
    }

    public bool CheckWord(string word)
    {
        Debug.Log(word);
        
        if ("oğuz".Equals(word))
        {
            return true;
        }
        else if ("yaşar".Equals(word))
        {
            return true;
        }
        else
        {
            return false;
        }
            
    }
/*
    public bool CheckWord(string word)
    {
        Debug.Log(word);
        bool value;
        if (bigwordlist1.words1.TryGetValue(s_enteredWord.ToLower(), out value))
        {
            return true;
        }
        else if (bigwordlist2.words2.TryGetValue(s_enteredWord.ToLower(), out value))
        {
            return true;
        }
        else if (bigwordlist3.words3.TryGetValue(s_enteredWord.ToLower(), out value))
        {
            return true;
        }
        else
            return false;
    }
    */
}
