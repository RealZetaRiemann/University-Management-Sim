using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student
{
    public string Name;
    public int SemsCompleted;
    public string Major;
    public int RequiredRemaining;
    public int ElectivesRemaining;
    public List<Course> CoursesTaken;
    public List<Course> CoursesTaking;

    public Student(string newName, int newSemsCompleted, string newMajor, int newRequiredRemaining, int newElectivesRemaining,
    List<Course> newCoursesTaken, List<Course> newCoursesTaking) 
    {
        Name = newName;
        SemsCompleted = newSemsCompleted;
        Major = newMajor;
        RequiredRemaining = newRequiredRemaining;
        ElectivesRemaining = newElectivesRemaining;
        CoursesTaken = newCoursesTaken;
        CoursesTaking = newCoursesTaking;
    }
}