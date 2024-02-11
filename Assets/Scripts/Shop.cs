using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Shop : MonoBehaviour
{
    public CourseList CL;
    public MajorsList ML;
    public TMP_Text ShopText;
    public List<Professor> ProfApps;
    public Player player;
    public List<bool> AlreadyHired;
    public List<bool> AlreadyAdmitted;
    public GameObject Errorpanel;
    public GameObject Hirepanel;
    public GameObject Staffpanel;
    public GameObject Admitpanel;
    public GameObject Coursepanel;
    public GameObject CourseChoicepanel;
    public GameObject YourStudentspanel;
    public GameObject HireHelppanel;
    public GameObject AdmitHelppanel;
    public GameObject ChooseHelppanel;
    public GameObject WinLosepanel;
    public TMP_Text WinLoseText;
    public TMP_Text StaffText;
    public TMP_Text AllCourseText;
    public TMP_Text YourCourseText;
    public TMP_Text YourStudentsText;
    public TMP_Text toAdmitText;
    public TMP_Text numAdmittedText;
    public List<Student> StudentApps;
    public int CurrentStudentApp;
    public int numAdmitted;
    public TMP_Text numCurrentStudents;
    public TMP_Text ErrorText;
    public bool firstAdmissions;
    public TMP_Text numStudentApplications;
    public TMP_Text GPText;
    public TMP_Text PrestigeText;
    public TMP_Text AcademicsText;
    public TMP_Text ChoosingCoursesText;
    public int CurrentProfCourseList;
    public Button HireStaffMenu;
    public Button AdmitStudentsMenu;
    public Button ChooseCoursesMenu;
    public List<List<bool>> CourseChoicesToggled;
    public Toggle CourseToggle1;
    public Toggle CourseToggle2;
    public Toggle CourseToggle3;
    public Toggle CourseToggle4;
    public Toggle CourseToggle5;
    public List<(Course, int, List<Student>)> CoursesRunning;
    public TMP_Text SummaryText;
    public GameObject Summarypanel;


    public Shop() {
        ProfApps = new List<Professor>();
        AlreadyHired = new List<bool>();
        AlreadyAdmitted = new List<bool>();
        StudentApps = new List<Student>();
        CurrentStudentApp = 0;
        numAdmitted = 0;
        firstAdmissions = false;
        CurrentProfCourseList = 0;
        CourseChoicesToggled = new List<List<bool>>();
        CoursesRunning = new List<(Course, int, List<Student>)>();
    }


    public void HideErrorPanel() {
        Errorpanel.SetActive(false);
    }


    //********** WINNING & LOSING ***********//



    // checks if they player has won or lost & displays the correct panel if either is true
    public void checkWinLoss() {
        if (player.GP >= 1000000 && player.Academics >= 100 && player.Prestige >= 100) {
            WinLosepanel.SetActive(true);
            WinLoseText.text = "YOU WIN!" + "<size=60%>\nGP: " + player.GP.ToString() +
            "\nAcademics: " + player.Academics.ToString() + "\nPrestige: " + player.Prestige.ToString();
        }
        else if (player.GP < 0 || player.Academics < 0 || player.Prestige < 0) {
            WinLosepanel.SetActive(true);
            WinLoseText.text = "YOU LOSE!" + "<size=60%>\nGP: " + player.GP.ToString() +
            "\nAcademics: " + player.Academics.ToString() + "\nPrestige: " + player.Prestige.ToString();
        }
    }


    //****** GENERATING PROFESSORS ******//




    // picks a random major from all the majors in the game
    public int randomMajorFromAll() {
        return Random.Range(0,ML.majors.Count);
    }

    // picks a random major from only the player's current available majors
    public int randomMajorFromAvailable() {
        return Random.Range(0,player.AvailableMajors.Count);
    }



    // build and return a new professor that has a random major specialty
    // and can teach one random required course from that major
    // and four other random courses from that major
    // and a random name
    public Professor MakeProfessor(int MajorIndex, string fromWhere) {

        string major = "";
        int firstCourseIndex = 0;

        if (fromWhere == "All") {
            major = ML.majors[MajorIndex].Item1; // get major key from list
            firstCourseIndex = ML.majors[MajorIndex].Item2; // get index of first course in that major from list
        }
        else {
            major = player.AvailableMajors[MajorIndex].Item1; // get major key from list
            firstCourseIndex = player.AvailableMajors[MajorIndex].Item2; // get index of first course in that major from list
        }

        List<Course> TeachableCourses = new List<Course>();

        // get index of random required course from major
        // each major has 5 required courses and they are always the first five in that major's
        // 'section' of the list, see comment in MajorsList.cs for details

        // check if player has all the required courses for this professor's major already 
        // and keep track of the ones they don't
        int playerHasAllReqs = 0;
        List<int> doesntHaveReq = new List<int>();
        for (int rc = firstCourseIndex; rc < firstCourseIndex + 5; rc++) {
            if (player.AvailableCourses.Exists(Course => Course.CourseCode == CL.allCourses[rc].CourseCode)) {
                playerHasAllReqs++;
            }
            else {
                doesntHaveReq.Add(rc);
            }
        }

        int randomRequired = 0;
        // if the player already has all the reqs, just pick one randomly
        if (playerHasAllReqs == 5) {
            randomRequired = Random.Range(firstCourseIndex,firstCourseIndex + 5);
        }
        // otherwise pick one randomly from what the player doesn't already have
        else {
            randomRequired = doesntHaveReq[Random.Range(0,doesntHaveReq.Count)];
        }

        TeachableCourses.Add(CL.allCourses[randomRequired]); // add required course to list

        for (int i = 0; i <= 3; i++)
        {
            // get random course index from major (there are 30 courses in each major)
            int randomOther = Random.Range(firstCourseIndex,firstCourseIndex + 30);
            // if the course isn't already in the major then add it
            if (TeachableCourses.Contains(CL.allCourses[randomOther]) == false) {
                TeachableCourses.Add(CL.allCourses[randomOther]);
            }
            // otherwise, try again
            else {
                i--;
            }
        }

        TextAsset jsonFile = Resources.Load<TextAsset>("FirstNames"); // get json file with list of names
        List<string> possibleNames = JsonConvert.DeserializeObject<List<string>>(jsonFile.text); // convert to list of strings
        int randomName = Random.Range(0,possibleNames.Count); // get index of random name from list
        string profName = "Professor " + possibleNames[randomName]; // make professor's name from random index

        return new Professor(profName, major, MajorIndex, TeachableCourses, 0, 30);
    }

    // makes three new professors and display them in text
    public void PopulateProfessorApplications() {
        List<Professor> AvailableProfs = new List<Professor>();
        string ShopContent = "";
        for (int i = 0; i < 3; i++) {
            // if the player already has some majors available,
            // make sure that at least the first professor's specialty is one already aquired
            if (player.AvailableMajors.Count > 0 && i == 0) {
                AvailableProfs.Add(MakeProfessor(randomMajorFromAvailable(), "Player's"));
            }
            // otherwise just pick a random specialty
            else {
                AvailableProfs.Add(MakeProfessor(randomMajorFromAll(), "All"));
            }
            ShopContent = ShopContent + "<indent=0%><size=120%>" + AvailableProfs[i].Name + "</size>\n<size=110%>" + AvailableProfs[i].Specialty + "</size>\n<indent=5%>";
            foreach (Course course in AvailableProfs[i].Courses) {
                ShopContent = ShopContent + "- " + course.CourseCode + " " + course.CourseTitle;
                if (course.Req == true) {
                    ShopContent = ShopContent + ", REQ";
                }
                if (course.PreReq != "None") {
                    ShopContent = ShopContent + ", PRQ";
                }
                ShopContent = ShopContent + "\n";
            }
            AlreadyHired[i] = false;
            ShopContent = ShopContent + "\n";
        }

        ProfApps = AvailableProfs;
        ShopText.text = ShopContent;
    }




    //******* DISPLAYING STAFF & COURSES ********//



    // parse the player's list of staff and display them in text
    public void ParseStaff() {
        string StringBuilder = "";
        if (player.EnlistedProfs.Count != 0) {
            for (int i = 0; i < player.EnlistedProfs.Count; i++) {
                StringBuilder = StringBuilder + "<indent=0%><size=120%>" + player.EnlistedProfs[i].Name + 
                "</size>\n<size=100%>" + player.EnlistedProfs[i].Specialty + "</size>\n<indent=5%><size=80%>";
                foreach (Course course in player.EnlistedProfs[i].Courses) {
                    StringBuilder = StringBuilder + "- " + course.CourseCode + " " + course.CourseTitle;
                    if (course.Req == true) {
                        StringBuilder = StringBuilder + ", REQ";
                    }
                    if (course.PreReq != "None") {
                        StringBuilder = StringBuilder + ", PRQ";
                    }
                    StringBuilder = StringBuilder + "\n";
                }
                StringBuilder = StringBuilder + "\n";
            }
        }
        else {
            StringBuilder = "<size=120%>No professors have been hired yet!";
        }
        StaffText.text = StringBuilder;
    }

    // parse all courses to be displayed in text
    public void ParseAllCourses() {
        string StringBuilder = "";
        int majorTracker = 0;
        for (int i = 0; i < CL.allCourses.Count; i++) {
            // since there are only 30 courses per major, add title text for the major every 30th course
            if (majorTracker % 30 == 0) {
                StringBuilder = StringBuilder + "<indent=0%><size=125%><align=\"center\">" + CL.allCourses[i].Major + "</size></align>\n\n";
            }
            // put course code : course title together
            StringBuilder = StringBuilder + "<indent=0%><size=110%>" + CL.allCourses[i].CourseCode + ": " +
            CL.allCourses[i].CourseTitle + "</size><indent=5%>";
            // if any courses are required or have prereqs then add that information
            if (CL.allCourses[i].Req == true) {
                StringBuilder = StringBuilder + "\n- Required";
            }
            if (CL.allCourses[i].PreReq != "None") {
                StringBuilder = StringBuilder + "\n- Prerequisite: " + CL.allCourses[i].PreReq;
            }
            // add maximum student information
            StringBuilder = StringBuilder + "\n- Maximum Students: " + CL.allCourses[i].StudentMaximum + "\n\n";
            majorTracker++;
        }
        AllCourseText.text = StringBuilder;
    }

    // parse the player's list of available courses and display them in text
    public void ParseYourCourses() {
        // sort courses alphabetically by their major
        IEnumerable<Course> alphabeticalByMajor = player.AvailableCourses.OrderBy(course => course.Major).ThenBy(course => course.CourseCode);
        string StringBuilder = "";
        string majorTracker = "";
        int timesTeachable = 1;
        for (int i = 0; i < alphabeticalByMajor.Count(); i++) {

            // every time we get to a course with a new major, add the title text for that major
            if (alphabeticalByMajor.ElementAt(i).Major != majorTracker) {
                StringBuilder = StringBuilder + "<indent=0%><size=125%><align=\"center\">" + alphabeticalByMajor.ElementAt(i).Major + "</size></align>\n\n";
                majorTracker = alphabeticalByMajor.ElementAt(i).Major;
            }

            // courses are sorted alphabetically by major, then course code... so any duplicate courses will be adjacent
            // check if the course is a duplicate of the previous course, if not then parse the new course info to text
            if (i == 0 || alphabeticalByMajor.ElementAt(i).CourseCode != alphabeticalByMajor.ElementAt(i-1).CourseCode) {
                // reset number of times teachable (must be a new course)
                timesTeachable = 1;
                // put course code : course title together
                StringBuilder = StringBuilder + "<indent=0%><size=110%>" + alphabeticalByMajor.ElementAt(i).CourseCode + ": " +
                alphabeticalByMajor.ElementAt(i).CourseTitle + "</size><indent=5%>";
                // if any courses are required or have prereqs then add that information
                if (alphabeticalByMajor.ElementAt(i).Req == true) {
                    StringBuilder = StringBuilder + "\n- Required";
                }
                if (alphabeticalByMajor.ElementAt(i).PreReq != "None") {
                    StringBuilder = StringBuilder + "\n- Prerequisite: " + alphabeticalByMajor.ElementAt(i).PreReq;
                }
                // add maximum student information
                StringBuilder = StringBuilder + "\n- Maximum Students: " + alphabeticalByMajor.ElementAt(i).StudentMaximum + "\n\n";
            }
            
            // ...but if we do find a duplicate course then add text to acknowledge that it can be taught by multiple professors
            else {
                // if the course *after* this course in the list is also a duplicate, wait and don't add the text yet
                // the number of times teachable it's still needs to increase
                if (i < alphabeticalByMajor.Count()-1 && alphabeticalByMajor.ElementAt(i).CourseCode == alphabeticalByMajor.ElementAt(i+1).CourseCode) {
                    timesTeachable += 1;
                }
                // otherwise, we know that the course before is the same and the one after either doesn't exist or is different
                // so add the number of times it's teachable
                else {
                    timesTeachable += 1;
                    StringBuilder = StringBuilder.Remove(StringBuilder.Length - 2, 2);  // get rid of extra newlines
                    StringBuilder = StringBuilder + "\n- Teachable: x" + timesTeachable.ToString() + "\n\n";
                }
            }
        }
        
        if (StringBuilder == "") {
            StringBuilder = "<size=120%>You have no courses yet!";
        }
        YourCourseText.text = StringBuilder;
    }




    //********** GENERATING & DISPLAYING STUDENTS ***********//





    public void ParseYourStudents() {
        // sort courses by major then by year
        IEnumerable<Student> studentsSorted = player.CurrentStudents.OrderBy(student => student.Major).ThenBy(student => student.SemsCompleted);
        string StringBuilder = "";
        string majorTracker = "";

        for (int i = 0; i < studentsSorted.Count(); i++) {
            // don't include students that have graduated or dropped out
            if (studentsSorted.ElementAt(i).SemsCompleted < 8) {
                // every time we get to a course with a new major, add the title text for that major
                if (studentsSorted.ElementAt(i).Major != majorTracker) {
                    StringBuilder = StringBuilder + "<indent=0%><size=125%><align=\"center\">" + studentsSorted.ElementAt(i).Major + "</size></align>\n\n";
                    majorTracker = studentsSorted.ElementAt(i).Major;
                }

                StringBuilder = StringBuilder + "<indent=0%><size=120%>" + studentsSorted.ElementAt(i).Name + "\n<size=110%><indent=5%>- " +
                studentsSorted.ElementAt(i).SemsCompleted.ToString() + " Semesters Completed\n- " +
                studentsSorted.ElementAt(i).RequiredRemaining.ToString() + " Required Courses Remaining\n- " + 
                studentsSorted.ElementAt(i).ElectivesRemaining.ToString() + " Elective Courses Remaining\n\n";
            }
        }

        if (StringBuilder == "") {
            StringBuilder = "<size=120%>You have no students yet!";
        }

        YourStudentsText.text = StringBuilder;

    }

    // build and return a new student with a random major and random name
    // all new students are currently freshman who have taken NO courses yet
    public Student MakeStudent(int MajorIndex, string fromWhere) {

        string major = "";
        int firstCourseIndex = 0;

        if (fromWhere == "All") {
            major = ML.majors[MajorIndex].Item1; // get major key from list
            firstCourseIndex = ML.majors[MajorIndex].Item2; // get index of first course in that major from list
        }
        else {
            major = player.AvailableMajors[MajorIndex].Item1; // get major key from list
            firstCourseIndex = player.AvailableMajors[MajorIndex].Item2; // get index of first course in that major from list
        }

        List<Course> CoursesTaken = new List<Course>();
        List<Course> CoursesTaking = new List<Course>();

        TextAsset jsonFile = Resources.Load<TextAsset>("FirstNames"); // get json file with list of names
        List<string> possibleNames = JsonConvert.DeserializeObject<List<string>>(jsonFile.text); // convert to list of strings
        int randomFirstName = Random.Range(0,possibleNames.Count); // get index of random last name from list
        int randomLastName = Random.Range(0,possibleNames.Count); // get index of random first name from list
        string studentName = possibleNames[randomFirstName] + " " + possibleNames[randomLastName]; // make student's name from random index

        return new Student(studentName, 0, major, 5, 10, CoursesTaken, CoursesTaking);
    }

    // makes X new students based on the player's presige
    public void NewStudentApplications() {
        int numApplicants = 20 + (3 * player.Prestige);
        for (int i = 0; i < numApplicants; i++) {
            if (i % 7 == 0) {
                StudentApps.Add(MakeStudent(randomMajorFromAll(), "All"));
            }
            else {
                StudentApps.Add(MakeStudent(randomMajorFromAvailable(), "Player's"));
            }
        }
        numStudentApplications.text = numApplicants.ToString() + " Applications";
        numAdmittedText.text = "0 Admitted";
        numCurrentStudents.text = player.CurrentStudents.Count.ToString() + " Total Students";
    }

    // displays the information of one student from a list with the given index i
    public void DisplayStudent(List<Student> StudentsList, int i) {
        string DisplayContent = "";
        DisplayContent = DisplayContent + "<size=80%>" + StudentsList[i].Name + "</size>\n<size=50%>" + "Major: " + StudentsList[i].Major;
        toAdmitText.text = DisplayContent;
    }




    //******* CHOOSING COURSES ********//



    // displays a professor with the courses they can teach
    public void DisplayCourseChoices(int p) {
        string DisplayContent = "";
        DisplayContent = DisplayContent + "<size=60%>" + player.EnlistedProfs[p].Name
                        + "\n<size=50%>" + player.EnlistedProfs[p].Specialty + "\n<size=35%><align=\"left\"><indent=190>";
        for (int i = 0; i < player.EnlistedProfs[p].Courses.Count; i++) {
            DisplayContent = DisplayContent + "\n" + player.EnlistedProfs[p].Courses[i].CourseCode + " " + player.EnlistedProfs[p].Courses[i].CourseTitle;
            if (player.EnlistedProfs[p].Courses[i].Req == true) {
                DisplayContent = DisplayContent + ", REQ";
            }
            DisplayContent = DisplayContent + "\nPrereq: " + player.EnlistedProfs[p].Courses[i].PreReq + "\n";
        }
        ChoosingCoursesText.text = DisplayContent;
    }

    // makes list of 5 bools set to false
    public List<bool> makeBoolList() {
        List<bool> boolList = new List<bool>();
        for (int i = 0; i < 5; i++) {
            boolList.Add(false);
        }
        return boolList;
    }

    // reset CourseChoicesToggled List
    public void SetUpChoiceToggle() {
        CourseChoicesToggled.Clear();
        for (int i = 0; i < player.EnlistedProfs.Count; i++) {
                CourseChoicesToggled.Add(makeBoolList());
            }
    }

    // when the player clicks a toggle check if it's on or off and change its value if possible
    // as well as its value in CourseChoicesToggled
    // (if three or more in the group are already on then a fourth cannot also be clicked on)
    public void ToggleOnClick(Toggle toggle, int toggleNum) {
        // check how many toggles are currently on (there are five in total)
        int totalSelected = 0;
        for (int i = 0; i < 5; i++) {
            if (CourseChoicesToggled[CurrentProfCourseList][i] == true) {
                totalSelected++;
            }
        }
        // if toggle is off and less than 3 toggles are selected, turn it on...
        // and log that the player is choosing to run that course
        if ((totalSelected < 3) && (CourseChoicesToggled[CurrentProfCourseList][toggleNum] == false)) {
            toggle.SetIsOnWithoutNotify(true);
            CourseChoicesToggled[CurrentProfCourseList][toggleNum] = true;
        }
        // if the toggle is on, turn it off...
        // and log that the player is choosing not to run that course
        else if (CourseChoicesToggled[CurrentProfCourseList][toggleNum] == true) {
            toggle.SetIsOnWithoutNotify(false);
            CourseChoicesToggled[CurrentProfCourseList][toggleNum] = false;
        }
        // otherwise... the player must be trying to turn on a toggle even though three are already on
        // error!
        else {
            ErrorText.text = "Professors can only teach up to three courses per semester!";
            Errorpanel.SetActive(true);
            Invoke("HideErrorPanel", 1f);
            toggle.SetIsOnWithoutNotify(false);
        }
    }

    // change whether the five toggles are on or off based on the information stored in the CourseChoicesToggled List
    public void setTogglestoNewProf() {
        Toggle[] toggles = {CourseToggle1, CourseToggle2, CourseToggle3, CourseToggle4, CourseToggle5};
        for (int i = 0; i < 5; i++) {
            if (CourseChoicesToggled[CurrentProfCourseList][i] == true) {
                toggles[i].SetIsOnWithoutNotify(true);
            }
            else {
                toggles[i].SetIsOnWithoutNotify(false);
            }
        }
    }
    // methods that call ToggleOnClick for each individual toggle 1-5
    public void ToggleOnClick1() {
        ToggleOnClick(CourseToggle1, 0);
    }
    public void ToggleOnClick2() {
        ToggleOnClick(CourseToggle2, 1);
    }
    public void ToggleOnClick3() {
        ToggleOnClick(CourseToggle3, 2);
    }
    public void ToggleOnClick4() {
        ToggleOnClick(CourseToggle4, 3);
    }
    public void ToggleOnClick5() {
        ToggleOnClick(CourseToggle5, 4);
    }




    //****** HIRING & ADMISSIONS ******//




    // hire professor (on click)
    public void HireProf(int profIndex) {
        if (AlreadyHired[profIndex] == false) {
            player.EnlistedProfs.Add(ProfApps[profIndex]);
            AlreadyHired[profIndex] = true;
            // add professor's teachable courses to list of player's available courses
            for (int i = 0; i < ProfApps[profIndex].Courses.Count; i++) {
                player.AvailableCourses.Add(ProfApps[profIndex].Courses[i]);
            }
            // check if professor's specialty is new
            if (!player.AvailableMajors.Exists(major => major.Item1 == ProfApps[profIndex].Specialty)) {
                player.Academics += 5;
                AcademicsText.text = "ACADEMICS: " + player.Academics.ToString();
                checkWinLoss(); // check for win
                // add professor's specialty to list of player's available majors
                player.AvailableMajors.Add(ML.majors[ProfApps[profIndex].SpecialtyIndex]);
            }

            // check if this is the 5th professor of a specialty
            int numOfThisSpecialty = 0;
            for (int p = 0; p < player.EnlistedProfs.Count; p++) {
                if (player.EnlistedProfs[p].Specialty == ProfApps[profIndex].Specialty) {
                    numOfThisSpecialty++;
                }
            }
            if (numOfThisSpecialty == 5) {
                player.Academics += 15;
                checkWinLoss(); // check for win
                AcademicsText.text = "ACADEMICS: " + player.Academics.ToString();
            }
        }
        // when the player tries to hire a professor that has already been hired, display error message
        else {
            ErrorText.text = "This professor has already been hired!";
            Errorpanel.SetActive(true);
            Invoke("HideErrorPanel", 1f);
        }
    }

    // admit student (on click)
    public void AdmitStudent() {
        if (AlreadyAdmitted[CurrentStudentApp] == false) {
            player.CurrentStudents.Add(StudentApps[CurrentStudentApp]);
            AlreadyAdmitted[CurrentStudentApp] = true;
            numAdmitted++;
            numAdmittedText.text = numAdmitted.ToString() + " Admitted";
            numCurrentStudents.text = player.CurrentStudents.Count.ToString() + " Total Students";
        }
        // when the player tries to admit a student that has already been admitted, display error message
        else {
            ErrorText.text = "This student has already been admitted!";
            Errorpanel.SetActive(true);
            Invoke("HideErrorPanel", 1f);
        }
    }




    //********* MENU BUTTONS **********//



    // show hire menu UI, hide others
    public void GoToHireMenu() {
        Staffpanel.SetActive(false);
        Coursepanel.SetActive(false);
        Hirepanel.SetActive(true);
        Admitpanel.SetActive(false);
        CourseChoicepanel.SetActive(false);
        YourStudentspanel.SetActive(false);
        HireHelppanel.SetActive(false);
        AdmitHelppanel.SetActive(false);
        ChooseHelppanel.SetActive(false);
        Summarypanel.SetActive(false);
    }

    // show staff menu UI, hide others
    public void GoToStaffMenu() {
        ParseStaff();
        Hirepanel.SetActive(false);
        Coursepanel.SetActive(false);
        Staffpanel.SetActive(true);
        Admitpanel.SetActive(false);
        CourseChoicepanel.SetActive(false);
        YourStudentspanel.SetActive(false);
        HireHelppanel.SetActive(false);
        AdmitHelppanel.SetActive(false);
        ChooseHelppanel.SetActive(false);
        Summarypanel.SetActive(false);
    }

    // show course menu UI, hide others
    public void GoToCourseMenu() {
        ParseYourCourses();
        Staffpanel.SetActive(false);
        Hirepanel.SetActive(false);
        Coursepanel.SetActive(true);
        Admitpanel.SetActive(false);
        CourseChoicepanel.SetActive(false);
        YourStudentspanel.SetActive(false);
        HireHelppanel.SetActive(false);
        AdmitHelppanel.SetActive(false);
        ChooseHelppanel.SetActive(false);
        Summarypanel.SetActive(false);
    }

    // show admit menu UI, hide others
    public void GoToAdmitMenu() {
        Staffpanel.SetActive(false);
        Hirepanel.SetActive(false);
        Coursepanel.SetActive(false);
        Admitpanel.SetActive(true);
        CourseChoicepanel.SetActive(false);
        YourStudentspanel.SetActive(false);
        HireHelppanel.SetActive(false);
        AdmitHelppanel.SetActive(false);
        ChooseHelppanel.SetActive(false);
        Summarypanel.SetActive(false);
    }

    // show course choosing menu UI, hide others
    public void GoToCourseChoosingMenu() {
        Staffpanel.SetActive(false);
        Hirepanel.SetActive(false);
        Coursepanel.SetActive(false);
        Admitpanel.SetActive(false);
        CourseChoicepanel.SetActive(true);
        YourStudentspanel.SetActive(false);
        HireHelppanel.SetActive(false);
        AdmitHelppanel.SetActive(false);
        ChooseHelppanel.SetActive(false);
        Summarypanel.SetActive(false);
    }

    // show students menu UI, hide others
    public void GoToYourStudentsMenu() {
        ParseYourStudents();
        Staffpanel.SetActive(false);
        Hirepanel.SetActive(false);
        Coursepanel.SetActive(false);
        Admitpanel.SetActive(false);
        CourseChoicepanel.SetActive(false);
        YourStudentspanel.SetActive(true);
        HireHelppanel.SetActive(false);
        AdmitHelppanel.SetActive(false);
        ChooseHelppanel.SetActive(false);
        Summarypanel.SetActive(false);
    }

    // show or hide help panels
    public void ShowHireHelp() {
        HireHelppanel.SetActive(true);
    }
    public void HideHireHelp() {
        HireHelppanel.SetActive(false);
    }
    public void ShowAdmitHelp() {
        AdmitHelppanel.SetActive(true);
    }
    public void HideAdmitHelp() {
        AdmitHelppanel.SetActive(false);
    }
    public void ShowChooseHelp() {
        ChooseHelppanel.SetActive(true);
    }
    public void HideChooseHelp() {
        ChooseHelppanel.SetActive(false);
    }


    // ******** OTHER BUTTONS *********//



    // display the previous student application if there is one
    public void AdmitPreviousOnClick() {
        if (CurrentStudentApp != 0) {
            CurrentStudentApp--;
        }
        else {
            ErrorText.text = "End of applications";
            Errorpanel.SetActive(true);
            Invoke("HideErrorPanel", 1f);            
        }
        DisplayStudent(StudentApps, CurrentStudentApp);
    }

    // display the next student application if there is one
    public void AdmitNextOnClick() {
        if (CurrentStudentApp < StudentApps.Count - 1) {
            CurrentStudentApp++;
        }
        else {
            ErrorText.text = "End of applications";
            Errorpanel.SetActive(true);
            Invoke("HideErrorPanel", 1f);            
        }
        DisplayStudent(StudentApps, CurrentStudentApp);
    }

    // display the previous professor's course options if there is one
    public void ChoosePreviousOnClick() {
        if (CurrentProfCourseList != 0) {
            CurrentProfCourseList--;
        }
        DisplayCourseChoices(CurrentProfCourseList);
        // reset toggles to what they should be for the previous prof
        setTogglestoNewProf();
    }

    // display the next professor's course options if there is one
    public void ChooseNextOnClick() {
        if (CurrentProfCourseList < player.EnlistedProfs.Count - 1) {
            CurrentProfCourseList++;
        }
        DisplayCourseChoices(CurrentProfCourseList);
        // reset toggles to what they should be for the next prof
        setTogglestoNewProf();
    }

    // end the hiring professors phase, go to admit students menu
    public void doneHiringOnClick() {
        if (player.EnlistedProfs.Count > 0) {
            NewStudentApplications();
            DisplayStudent(StudentApps, 0);
            GoToAdmitMenu();
            HireStaffMenu.interactable = false;
            AdmitStudentsMenu.interactable = true;
        }
        else {
            ErrorText.text = "You must hire at least one professor!";
            Errorpanel.SetActive(true);
            Invoke("HideErrorPanel", 1f);            
        }
    }

    // end the admitting students phase, go to choose courses menu
    public void doneAdmittingOnClick() {
        // update prestige
        int numApplicants = 20 + (3 * player.Prestige);
        if (numAdmitted > numApplicants * 0.75) {
            player.Prestige -= 2;
        }
        else if (numAdmitted > numApplicants * 0.5) {
            player.Prestige -= 1;
        }
        else if (numAdmitted > numApplicants * 0.25) {
            player.Prestige += 1;
        }
        else if (numAdmitted > 0) {
            player.Prestige += 2;
        }
        PrestigeText.text = "PRESTIGE: " + player.Prestige.ToString();
        checkWinLoss(); // check for win/loss
        numAdmitted = 0; // reset numAdmitted
        StudentApps.Clear(); // reset StudentApps
        GoToCourseChoosingMenu();
        AdmitStudentsMenu.interactable = false;
        ChooseCoursesMenu.interactable = true;
        DisplayCourseChoices(0);
        SetUpChoiceToggle();
    }

    // hide summary panel (on click)
    public void hideSummary() {
        Summarypanel.SetActive(false);
    }

    // show summary panel after course choices are made and the semester has ended
    public void showSummary(int GPgained, int GPlost, int numDroppedOut, int numGraduated) {
        string StringBuilder = "";
        StringBuilder = StringBuilder + GPgained.ToString() + " GP Gained\n" + GPlost.ToString() + " GP Lost\n\n";
        StringBuilder = StringBuilder + numDroppedOut.ToString() + " Students Dropped Out (Prestige Lost)\n" + numGraduated.ToString() + " Graduated";
        for (int c = 0; c < CoursesRunning.Count; c++) {
            StringBuilder = StringBuilder + "\n\n<indent=0>" + CoursesRunning[c].Item3.Count.ToString() + " Students Taking " + CoursesRunning[c].Item1.CourseCode + ": <indent=5%>\n";
            for (int s = 0; s < CoursesRunning[c].Item3.Count; s++) {
                StringBuilder = StringBuilder + "- " + CoursesRunning[c].Item3[s].Name + "\n";
            }
        }

        SummaryText.text = StringBuilder;
        Summarypanel.SetActive(true);
    }

    // end the course choosing phase, take GP from professors,
    // students choose which courses to take, take GP from students,
    // show summary panel, reset for next semester, go to hire professors menu
    public void doneChoosingCoursesOnClick() {

        int GPgained = 0;
        int GPlost = 0;
        CoursesRunning.Clear(); // reset courses running
        int numGraduated =  0;
        int numDroppedOut = 0;

        // for each running course add it to the list of courses running and it's student maximum
        // note that the same course can be on the list more than once if more than one prof is teaching it
        for (int prof = 0; prof < CourseChoicesToggled.Count; prof++) {
            for (int crs = 0; crs < 5; crs++) {
                if (CourseChoicesToggled[prof][crs] == true) {
                    Course crsObject = player.EnlistedProfs[prof].Courses[crs];
                    int stdtMax = player.EnlistedProfs[prof].Courses[crs].StudentMaximum;
                    List<Student> newList = new List<Student>();
                    CoursesRunning.Add((crsObject, stdtMax, newList));
                }
            }
        }

        // lose/pay professors 30GP per course running
        GPlost = (CoursesRunning.Count * 30);
        player.GP = player.GP - (CoursesRunning.Count * 30);

        // students choose courses to take
        for (int stdt = 0; stdt < player.CurrentStudents.Count; stdt++) {

            Student stdtObject = player.CurrentStudents[stdt];
            stdtObject.SemsCompleted++;
            // number of semesters completed must be less than 8 to keep taking courses next sem
            if (stdtObject.SemsCompleted < 8) {

                // move any courses taken the previous semester from CoursesTaking to CoursesTaken
                stdtObject.CoursesTaken.AddRange(stdtObject.CoursesTaking);
                // clear CoursesTaking
                stdtObject.CoursesTaking.Clear();

                List<(Course, int)> backups = new List<(Course, int)>();
                List<(Course, int)> backbackups = new List<(Course, int)>();

                for (int crs = 0; crs < CoursesRunning.Count; crs++) {

                    // students can only take up to 4 classes per semester
                    if (stdtObject.CoursesTaking.Count < 4) {

                        Course crsObject = CoursesRunning[crs].Item1;
                        int numSeats = CoursesRunning[crs].Item2; // must be > 0 to take course
                        bool alreadyTaken = stdtObject.CoursesTaken.Contains(crsObject); // must be false to take course
                        bool currentlyTaking = stdtObject.CoursesTaking.Contains(crsObject); // must be false to take course

                        // seats must be available and the student must not have already taken the course
                        if (numSeats > 0 && !alreadyTaken && !currentlyTaking) {
                            // either the course must not have a prereq or the student must have taken it
                            if (crsObject.PreReq == "None" || stdtObject.CoursesTaken.Exists(Course => Course.CourseCode == crsObject.PreReq)) {
                                // courses that are for the student's major are prioritized
                                if (crsObject.Major == stdtObject.Major) {
                                    // courses that are required are taken automatically
                                    if (crsObject.Req) {
                                        stdtObject.CoursesTaking.Add(crsObject);
                                        // update tuple with 1 less spot available
                                        CoursesRunning[crs].Item3.Add(stdtObject);
                                        var tempTup = CoursesRunning[crs];
                                        CoursesRunning[crs] = (tempTup.Item1, tempTup.Item2 - 1, tempTup.Item3);
                                        // update num required courses remaining for student
                                        stdtObject.RequiredRemaining--;
                                    }
                                    // elective major courses are saved as backups
                                    // note that an elective is any course for a major that is not required
                                    else {
                                        backups.Add((crsObject, crs));
                                    }
                                }
                                // any other non-major courses are saved as backups to the backups
                                else {
                                    backbackups.Add((crsObject, crs));
                                }
                            }
                        }
                    }
                }
                // if the student doesn't have enough classes to take just from reqs, iterate through the backups (electives) and add them
                if (stdtObject.CoursesTaking.Count < 4) {
                    for (int crs = 0; crs < backups.Count; crs++) {
                        // need to check this again so it doesn't go over..
                        if (stdtObject.CoursesTaking.Count < 4) {
                            stdtObject.CoursesTaking.Add(backups[crs].Item1);
                            // update CoursesRunning list tuple for this course with 1 less spot available
                            int CourseRunningIndex = backups[crs].Item2;
                            CoursesRunning[crs].Item3.Add(stdtObject);
                            var tempTup = CoursesRunning[CourseRunningIndex];
                            CoursesRunning[CourseRunningIndex] = (tempTup.Item1, tempTup.Item2 - 1, tempTup.Item3);
                            // update num elective courses remaining for student
                            stdtObject.ElectivesRemaining--;
                        }
                    }
                }
                // if the student doesn't have enough classes to take just from their major, iterate through the backbackups and add them
                if (stdtObject.CoursesTaking.Count < 4) {
                    for (int crs = 0; crs < backbackups.Count; crs++) {
                        // need to check this again so it doesn't go over..
                        if (stdtObject.CoursesTaking.Count < 4) {
                            stdtObject.CoursesTaking.Add(backbackups[crs].Item1);
                            // update CoursesRunning list tuple for this course with 1 less spot available
                            int CourseRunningIndex = backbackups[crs].Item2;
                            CoursesRunning[crs].Item3.Add(stdtObject);
                            var tempTup = CoursesRunning[CourseRunningIndex];
                            CoursesRunning[CourseRunningIndex] = (tempTup.Item1, tempTup.Item2 - 1, tempTup.Item3);
                        }
                    }
                }

                // once all a student's courses have been chosen, they pay 5GP per course
                GPgained = GPgained + stdtObject.CoursesTaking.Count * 5;
                player.GP = player.GP + (stdtObject.CoursesTaking.Count * 5);
            }

            // if number of semesters completed = 8 and they have *not* finished the reqs for their major...
            // then they drop out and the player loses a prestige
            else if (stdtObject.SemsCompleted == 8) {
                if (stdtObject.RequiredRemaining != 0 || stdtObject.ElectivesRemaining != 0) {
                    player.Prestige--;
                    numDroppedOut++;
                }
                else {
                    numGraduated++;
                }
            }
        }

        PrestigeText.text = "PRESTIGE: " + player.Prestige.ToString();
        GPText.text = "GP: " + player.GP.ToString();
        checkWinLoss(); // check for win/loss

        // next semester starts!
        for (int i = 0; i < 3; i++) {
            AlreadyHired[i] = false; // none of the professors have been hired yet
        }
        AlreadyAdmitted.Clear();
        int numApplicants = 20 + (3 * player.Prestige);
        for (int i = 0; i < numApplicants; i++) {
            AlreadyAdmitted.Add(false); // none of the students have been admitted yet
        }
        PopulateProfessorApplications(); // make and display professor application

        // reset course choosing toggles
        CourseToggle1.SetIsOnWithoutNotify(false);
        CourseToggle2.SetIsOnWithoutNotify(false);
        CourseToggle3.SetIsOnWithoutNotify(false);
        CourseToggle4.SetIsOnWithoutNotify(false);
        CourseToggle5.SetIsOnWithoutNotify(false);

        // go back to hiring menu
        ChooseCoursesMenu.interactable = false;
        HireStaffMenu.interactable = true;
        GoToHireMenu();
        showSummary(GPgained, GPlost, numDroppedOut, numGraduated);

    }



    

    //*********** START **********//




    // when starting the scene, build and display the staff hiring menu, hide all others
    void Start() {
        Hirepanel.SetActive(true);
        for (int i = 0; i < 3; i++) {
            AlreadyHired.Add(false); // none of the professors have been hired yet
        }
        for (int i = 0; i < 35; i++) {
            AlreadyAdmitted.Add(false); // none of the students have been admitted yet
        }
        PopulateProfessorApplications(); // make and display professor application
        ParseAllCourses(); // format list of all courses in text
        // hide all other panels
        Errorpanel.SetActive(false);
        Staffpanel.SetActive(false);
        Coursepanel.SetActive(false);
        Admitpanel.SetActive(false);
        AdmitStudentsMenu.interactable = false;
        ChooseCoursesMenu.interactable = false;
        CourseToggle1.SetIsOnWithoutNotify(false);
        CourseToggle2.SetIsOnWithoutNotify(false);
        CourseToggle3.SetIsOnWithoutNotify(false);
        CourseToggle4.SetIsOnWithoutNotify(false);
        CourseToggle5.SetIsOnWithoutNotify(false);
        // set initial player stats
        player.Prestige = 5;
        PrestigeText.text = "PRESTIGE: " + player.Prestige.ToString();
        player.GP = 100;
        GPText.text = "GP: " + player.GP.ToString();
    }


}