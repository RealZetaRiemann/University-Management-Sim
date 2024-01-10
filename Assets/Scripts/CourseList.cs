using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class CourseList : MonoBehaviour
{
    public List<Course> allCourses = new List<Course>();

    void Start() {

        // get list of courses
        TextAsset jsonFile = Resources.Load<TextAsset>("Courses");
        allCourses = JsonConvert.DeserializeObject<List<Course>>(jsonFile.text);

    }

}