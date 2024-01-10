using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Course
{
    public string CourseCode;
    public string CourseTitle;
    public string PreReq;
    public string Major;
    public bool Req;
    public int StudentMaximum;

    public Course(string newCourseCode, string newCourseTitle, string newPreReq, string newMajor, 
    bool newReq, int newStudentMaximum) 
    {
        CourseCode = newCourseCode;
        CourseTitle = newCourseTitle;
        PreReq = newPreReq;
        Major = newMajor;
        Req = newReq;
        StudentMaximum = newStudentMaximum;
    }
}