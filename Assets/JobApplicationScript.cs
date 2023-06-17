using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;

public class JobApplicationScript : MonoBehaviour {


  public KMBombInfo Bomb;
  public KMBombModule Module;
  public KMBombInfo BombInfo;
  public KMAudio Audio;
  public KMSelectable[] Buttons;
  public TextMesh[] DisplayTexts;
  public Renderer[] winButtons;
  public Material[] submitMats;

  //A tank of a fucking script. Jah have mercy upon me!

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;
   private string solveSoundSelection = "Solve1";
   private string strikeSoundSelection = "Strike1";

   private List<string> applicantNames = new List<string>()
{
    "Emily",
    "Jacob",
    "Sophia",
    "Danny",
    "Olivia",
    "Matthew",
    "Ava",
    "William",
    "Isabella",
    "Michael"
};

private string[] availabilityOptions = { "Part-Time", "Full-Time", "Flexible" };

private List<string> jobOptions = new List<string>()
{
    "Usher",
    "Retail",
    "Barista",
    "Babysitter",
    "Lifeguard",
    "Teacher",
    "Chef",
    "Musician",
    "Assistant",
    "CEO"
};

private List<string> companyNames = new List<string>()
{
    "Starlight Theaters",
    "Fashion Junction",
    "Café Perk",
    "Playful Sprouts Babysitting",
    "Aqua Oasis",
    "Bright Minds Academy",
    "Gourmet Delights",
    "Harmony Studios",
    "Dynamic Support Services",
    "Greenlight Startup"
};

private string GetCurrentCompanyName(int selectedJobIndex)
{
    if (selectedJobIndex >= 0 && selectedJobIndex < jobOptions.Count)
    {
        int companyIndex = selectedJobIndex;
        if (companyIndex >= 0 && companyIndex < companyNames.Count)
        {
            return companyNames[companyIndex];
        }
    }
    return "Invalid Job Index";
}

//10 names & jobs

  private int currentNameIndex;
  private int currentAvailabilityIndex;
  private int currentPositionIndex;
  private List<string> selectedChoices;

   void Awake () {
      ModuleId = ModuleIdCounter++;
      GetComponent<KMBombModule>().OnActivate += Activate;
      /*
      foreach (KMSelectable object in keypad) {
          object.OnInteract += delegate () { keypadPress(object); return false; };
      }
      */

      //right buttons
      Buttons[0].OnInteract += delegate () { clipboardPress(1); return false; }; //name
      Buttons[1].OnInteract += delegate () { clipboardPress(2); return false; }; //avail
      Buttons[2].OnInteract += delegate () { clipboardPress(3); return false; }; //position

      //right buttons
      Buttons[3].OnInteract += delegate () { clipboardPress(4); return false; }; //name
      Buttons[4].OnInteract += delegate () { clipboardPress(5); return false; }; //avail
      Buttons[5].OnInteract += delegate () { clipboardPress(6); return false; }; //position

      Buttons[6].OnInteract += delegate () { clipboardPress(7); return false; }; //submit answers
   }

   void establishSelection()
   {
       // Generate random indices within the valid range
       currentNameIndex = UnityEngine.Random.Range(0, applicantNames.Count);
       currentAvailabilityIndex = UnityEngine.Random.Range(0, availabilityOptions.Length);
       currentPositionIndex = UnityEngine.Random.Range(0, jobOptions.Count);


       // Update the display texts with the corresponding values
       DisplayTexts[0].text = applicantNames[currentNameIndex];
       DisplayTexts[1].text = availabilityOptions[currentAvailabilityIndex];
       DisplayTexts[2].text = jobOptions[currentPositionIndex];
   }

   void clipboardPress(int buttonIndex) {
     if(!ModuleSolved)
     {
     switch (buttonIndex) {
       case 1:  // name forward
         currentNameIndex = (currentNameIndex + 1) % applicantNames.Count;
         DisplayTexts[0].text = applicantNames[currentNameIndex];
         Audio.PlaySoundAtTransform("PaperFlip", DisplayTexts[3].transform);
         break;
       case 4:  // name backward
         currentNameIndex =
             (currentNameIndex - 1 + applicantNames.Count) % applicantNames.Count;
         DisplayTexts[0].text = applicantNames[currentNameIndex];
         Audio.PlaySoundAtTransform("PaperFlip", DisplayTexts[3].transform);
         break;
       case 2: //avail forward
         currentAvailabilityIndex =
             (currentAvailabilityIndex + 1) % availabilityOptions.Length;
         DisplayTexts[1].text = availabilityOptions[currentAvailabilityIndex];
         Audio.PlaySoundAtTransform("PaperFlip", DisplayTexts[3].transform);
         break;
       case 5: //avail back
         currentAvailabilityIndex =
             (currentAvailabilityIndex - 1 + availabilityOptions.Length) %
             availabilityOptions.Length;
         DisplayTexts[1].text = availabilityOptions[currentAvailabilityIndex];
         Audio.PlaySoundAtTransform("PaperFlip", DisplayTexts[3].transform);
         break;
       case 3: //position forward
         currentPositionIndex = (currentPositionIndex + 1) % jobOptions.Count;
         DisplayTexts[2].text = jobOptions[currentPositionIndex];
         Audio.PlaySoundAtTransform("PaperFlip", DisplayTexts[3].transform);
         break;
       case 6: //position back
         currentPositionIndex =
             (currentPositionIndex - 1 + jobOptions.Count) % jobOptions.Count;
         DisplayTexts[2].text = jobOptions[currentPositionIndex];
         Audio.PlaySoundAtTransform("PaperFlip", DisplayTexts[3].transform);
         break;
       case 7:
         Debug.Log("Submitted Name: " + DisplayTexts[0].text);
         Debug.Log("Submitted Availability: " + DisplayTexts[1].text);
         Debug.Log("Submitted Job Position: " + DisplayTexts[2].text);
         if (selectedChoices != null && DisplayTexts[0].text == selectedChoices[0] && DisplayTexts[1].text == selectedChoices[2] && DisplayTexts[2].text == selectedChoices[1])
         {
             DisplayTexts[3].text = "SUBMITTED!";
             ModuleSolved = true;
             Solve();
             StartFlashing();
             Audio.PlaySoundAtTransform(solveSoundSelection, DisplayTexts[3].transform);
         }
         else
         {
           int strikeSoundIndex = UnityEngine.Random.Range(1, 7); // MAKES RANDOM NUMBER FOR STRIKE SOUNDS OF ME TALKING
           switch (strikeSoundIndex)
           {
             case 1:
                 strikeSoundSelection = "Strike1";
                 break;
             case 2:
                 strikeSoundSelection = "Strike2";
                 break;
             case 3:
                 strikeSoundSelection = "Strike3";
                 break;
             case 4:
                 strikeSoundSelection = "Strike4";
                 break;
             case 5:
                 strikeSoundSelection = "Strike5";
                 break;
             case 6:
                 strikeSoundSelection = "Strike6";
                 break;
             default:
                 strikeSoundSelection = "Strike1";
                 break;
           }
            StartCoroutine(StrikeFlash());
            Audio.PlaySoundAtTransform(strikeSoundSelection, DisplayTexts[3].transform);
            Strike();
         }
      break;
       default:
         Debug.LogWarning("Invalid button index: " + buttonIndex);  // physically impossible?
         break;
     }
     SwitchCompanyName();
   }
 }


   void SwitchCompanyName() {
     string currentUpdatedCompanyName = GetCurrentCompanyName(currentPositionIndex);
     DisplayTexts[4].text = currentUpdatedCompanyName;
   }

   private float flashInterval = 0.08f;
   private float flashDuration = 2f;

   private Coroutine flashingCoroutine;

   private void StartFlashing()
   {
       if (flashingCoroutine != null)
           StopCoroutine(flashingCoroutine);
       flashingCoroutine = StartCoroutine(FlashMaterials());
   }

   private IEnumerator FlashMaterials()
   {
       Renderer[] renderers = new Renderer[winButtons.Length];
       Material[] originalMaterials = new Material[winButtons.Length];

       for (int i = 0; i < winButtons.Length; i++)
       {
           renderers[i] = winButtons[i].GetComponent<Renderer>();
           if (renderers[i] != null)
               originalMaterials[i] = renderers[i].material;
       }

       float elapsedTime = 0f;
       bool isMaterial1 = true;

       while (elapsedTime < flashDuration)
       {
           for (int i = 0; i < winButtons.Length; i++)
           {
               if (renderers[i] != null)
                   renderers[i].material = isMaterial1 ? submitMats[0] : submitMats[1];
           }
           yield return new WaitForSeconds(flashInterval);
           isMaterial1 = !isMaterial1;
           elapsedTime += flashInterval;
       }

       for (int i = 0; i < winButtons.Length; i++)
       {
           if (renderers[i] != null)
               renderers[i].material = originalMaterials[i];
       }
       winButtons[0].material = submitMats[0];
       winButtons[1].material = submitMats[0];
       winButtons[2].material = submitMats[0];
       winButtons[3].material = submitMats[0];
       winButtons[4].material = submitMats[0];
       winButtons[5].material = submitMats[0];
       winButtons[6].material = submitMats[0];
   }

   private float StrikeflashDuration = 0.5f;
   private float StrikeflashInterval = 0.15f; // Time interval between flashes
   IEnumerator StrikeFlash()
   {
       int numberOfFlashes = Mathf.CeilToInt(StrikeflashDuration / StrikeflashInterval);
       for (int i = 0; i < numberOfFlashes; i++)
       {
           winButtons[0].material = submitMats[2];
           winButtons[1].material = submitMats[2];
           winButtons[2].material = submitMats[2];
           winButtons[3].material = submitMats[2];
           winButtons[4].material = submitMats[2];
           winButtons[5].material = submitMats[2];
           winButtons[6].material = submitMats[2];
           yield return new WaitForSeconds(flashInterval);
           winButtons[0].material = submitMats[1];
           winButtons[1].material = submitMats[1];
           winButtons[2].material = submitMats[1];
           winButtons[3].material = submitMats[1];
           winButtons[4].material = submitMats[1];
           winButtons[5].material = submitMats[1];
           winButtons[6].material = submitMats[1];
           yield return new WaitForSeconds(flashInterval);
       }
   }

   void OnDestroy () { //Shit you need to do when the bomb ends

   }

   void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

   }

   private void GetChoicesForWeekday(DayOfWeek weekday, ref List<string> selectedChoices)
{
    selectedChoices.Clear(); // Clear the list before populating it

    switch (weekday)
    {
        case DayOfWeek.Monday:
            selectedChoices.Add(applicantNames[0]); //Emily
            selectedChoices.Add(jobOptions[0]); //Usher
            if (Bomb.IsPortPresent(Port.Serial))
                selectedChoices.Add(availabilityOptions[2]); // Flexible
            else
                selectedChoices.Add(availabilityOptions[0]); // Part-Time
            break;

            case DayOfWeek.Tuesday:
                selectedChoices.Add(applicantNames[1]); // Jacob
                selectedChoices.Add(jobOptions[4]); //Lifeguard
                if (Bomb.GetBatteryCount() == 2)
                    selectedChoices.Add(availabilityOptions[1]); // Full-Time
                else
                    selectedChoices.Add(availabilityOptions[2]); // Flexible
                break;

                case DayOfWeek.Wednesday:
                    selectedChoices.Add(applicantNames[7]); // William
                    selectedChoices.Add(jobOptions[1]); //Retail
                    if (Bomb.GetSerialNumberNumbers().Any(n => n % 2 == 0)) //even digit in serial number
                        selectedChoices.Add(availabilityOptions[0]); // Part-Time
                    else
                        selectedChoices.Add(availabilityOptions[1]); // Full-Time
                    break;

                    case DayOfWeek.Thursday:
                        selectedChoices.Add(applicantNames[4]); // Olivia
                        if (Bomb.GetSerialNumberLetters().Any(x => "XDFRH".Contains(x)))
                            selectedChoices.Add(jobOptions[8]); // Assistant
                        else
                            selectedChoices.Add(jobOptions[2]); // Barista
                        if (Bomb.IsPortPresent(Port.StereoRCA))
                            selectedChoices.Add(availabilityOptions[1]); // Full-Time
                        else
                            selectedChoices.Add(availabilityOptions[2]); // Flexible
                        break;

                        case DayOfWeek.Friday:
                        if (Bomb.GetSerialNumberLetters().Any(x => "AEIOU".Contains(x)))
                            selectedChoices.Add(applicantNames[3]); // Danny
                        else
                            selectedChoices.Add(applicantNames[6]); // Ava
                            selectedChoices.Add(jobOptions[7]); // Musician
                            if (Bomb.IsIndicatorOff(Indicator.CAR))
                                selectedChoices.Add(availabilityOptions[2]); // Flexible
                            else
                                selectedChoices.Add(availabilityOptions[0]); // Part-time
                            break;

                            case DayOfWeek.Saturday:
                                selectedChoices.Add(applicantNames[5]); //Matthew
                                selectedChoices.Add(jobOptions[3]); //Babysitter
                                if (Bomb.IsPortPresent(Port.PS2))
                                    selectedChoices.Add(availabilityOptions[1]); // Full-Time
                                else
                                    selectedChoices.Add(availabilityOptions[0]); // Part-time
                                break;

        case DayOfWeek.Sunday:
            selectedChoices.Add(applicantNames[7]); // Matthew
            selectedChoices.Add(jobOptions[5]); //Teacher of The Lord
            selectedChoices.Add(availabilityOptions[2]); // Flexible
            break;
    }
}

void Start() {
  //gets day of the week, starts selectedChoices (Which is really the correct answer) list
  DayOfWeek realWeekday = DateTime.Now.DayOfWeek;
  selectedChoices = new List<string>();
  GetChoicesForWeekday(realWeekday, ref selectedChoices);

  //sets random names, positions, and avail as the current text buttons
  establishSelection();

  //Finds current position, sets company name on start
  string startCompanyName = GetCurrentCompanyName(currentPositionIndex);
  DisplayTexts[4].text = startCompanyName;

  //Logs
  Debug.Log("Correct Name: " + selectedChoices[0]);
  Debug.Log("Correct Availability: " + selectedChoices[2]);
  Debug.Log("Correct Job Position: " + selectedChoices[1]);

  int solveSoundIndex = UnityEngine.Random.Range(1, 7); // MAKES RANDOM NUMBER FOR SOLVE SOUNDS OF ME TALKING
  switch (solveSoundIndex)
{
    case 1:
        solveSoundSelection = "Solve1";
        break;
    case 2:
        solveSoundSelection = "Solve2";
        break;
    case 3:
        solveSoundSelection = "Solve3";
        break;
    case 4:
        solveSoundSelection = "Solve4";
        break;
    case 5:
        solveSoundSelection = "Solve5";
        break;
    case 6:
        solveSoundSelection = "Solve6";
        break;
    default:
        solveSoundSelection = "Solve1";
        break;
}
}

   void Update () { //Shit that happens at any point after initialization

   }

   void Solve () {
      GetComponent<KMBombModule>().HandlePass();
   }

   void Strike () {
      GetComponent<KMBombModule>().HandleStrike();
   }

//TWITCH PLAYS Support

//NAMES
void PressNameButton(int nameIndex)
{
    int desiredIndex = nameIndex;
    if (desiredIndex < 0 || desiredIndex >= applicantNames.Count)
    {
        Debug.Log("Desired name not found in selectedChoices.");
        return;
    }

    int numPresses = (desiredIndex - currentNameIndex + applicantNames.Count) % applicantNames.Count;
    Debug.Log(numPresses);

    StartCoroutine(DelayedButtonPress(desiredIndex, numPresses));
}

    IEnumerator DelayedButtonPress(int desiredIndex, int numPresses)
        {
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(PressButtonUntilName(desiredIndex, numPresses));
            yield return new WaitForSeconds(0.3f);
        }

        IEnumerator PressButtonUntilName(int desiredIndex, int numPresses)
        {
            string desiredName = applicantNames[desiredIndex];
            string currentName = applicantNames[currentNameIndex];
            Debug.Log(desiredName + " and " + currentName);
            int presses = 0;
            while (currentName != desiredName && presses < numPresses)
            {
                Buttons[0].OnInteract();
                presses++;
                yield return new WaitForSeconds(0.3f);
            }
        }

//JOBS
void PressJobButton(int jobIndex)
{
    int desiredIndex = jobIndex;
    if (desiredIndex < 0 || desiredIndex >= jobOptions.Count)
    {
        Debug.Log("Desired name not found in jobOptions.");
        return;
    }

    int numPresses = (desiredIndex - currentPositionIndex + jobOptions.Count) % jobOptions.Count;
    Debug.Log(numPresses);
    StartCoroutine(DelayedJobButtonPress(desiredIndex, numPresses));
}

    IEnumerator DelayedJobButtonPress(int desiredIndex, int numPresses)
        {
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(PressButtonUntilJob(desiredIndex, numPresses));
            yield return new WaitForSeconds(0.3f);
        }

        IEnumerator PressButtonUntilJob(int desiredIndex, int numPresses)
        {
            string desiredJob = jobOptions[desiredIndex];
            string currentJob = applicantNames[currentPositionIndex];
            Debug.Log(desiredJob + " and " + currentJob);
            int presses = 0;
            while (currentJob != desiredJob && presses < numPresses)
            {
                Buttons[2].OnInteract();
                presses++;
                yield return new WaitForSeconds(0.3f);
            }
        }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} name [name] to select the correct name for the application. Use !{0} time [part,full,flexible] to select the availability for the application. Use !{0} job [job] to select the correct job for the application. Use !{0} submit to submit final answers.";
#pragma warning restore 414

IEnumerator ProcessTwitchCommand(string Command) {
  Command = Command.ToUpper();
  yield
  return null;

  switch (Command) {

  case "NAME EMILY":
    PressNameButton(0);
    break;
  case "NAME JACOB":
    PressNameButton(1);
    break;
  case "NAME SOPHIA":
    PressNameButton(2);
    break;
  case "NAME DANNY":
    PressNameButton(3);
    break;
  case "NAME OLIVIA":
    PressNameButton(4);
    break;
  case "NAME MATTHEW":
    PressNameButton(5);
    break;
  case "NAME AVA":
    PressNameButton(6);
    break;
  case "NAME WILLIAM":
    PressNameButton(7);
    break;
  case "NAME ISABELLA":
    PressNameButton(8);
    break;
  case "NAME MICHAEL":
    PressNameButton(9);
    break;

  case "JOB USHER":
    PressJobButton(0);
    break;
  case "JOB RETAIL":
    PressJobButton(1);
    break;
  case "JOB BARISTA":
    PressJobButton(2);
    break;
  case "JOB BABYSITTER":
    PressJobButton(3);
    break;
  case "JOB LIFEGUARD":
    PressJobButton(4);
    break;
  case "JOB TEACHER":
    PressJobButton(5);
    break;
  case "JOB CHEF":
    PressJobButton(6);
    break;
  case "JOB MUSICIAN":
    PressJobButton(7);
    break;
  case "JOB ASSISTANT":
    PressJobButton(8);
    break;
  case "JOB CEO":
    PressJobButton(9);
    break;

  case "TIME PART":
    currentAvailabilityIndex = 1;
    DisplayTexts[2].text = "PART-TIME";
    break;

  case "TIME FULL":
    currentAvailabilityIndex = 1;
    DisplayTexts[2].text = "FULL-TIME";
    break;

  case "TIME FLEXIBLE":
    currentAvailabilityIndex = 2;
    DisplayTexts[2].text = "FLEXIBLE";
    break;

  case "SUBMIT":
    Buttons[6].OnInteract();
    break;

  default:
    yield
    return string.Format(
      "sendtochaterror Invalid command");
    yield
    break;
  }
}

   IEnumerator TwitchHandleForcedSolve () {
      string forceSolveName = selectedChoices[0];
      string forceSolveJob = selectedChoices[1];
      string forceSolveTime = selectedChoices[2];
      DisplayTexts[4].text = "FORCE SOLVED";
      DisplayTexts[0].text = forceSolveName;
      DisplayTexts[1].text = forceSolveTime;
      DisplayTexts[2].text = forceSolveJob;
      DisplayTexts[3].text = "SUBMITTED!";
      ModuleSolved = true;
      Solve();
      StartFlashing();
      Audio.PlaySoundAtTransform(solveSoundSelection, DisplayTexts[3].transform);
      yield return null;
   }
}
