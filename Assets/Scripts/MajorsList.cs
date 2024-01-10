using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorsList : MonoBehaviour
{
    // the second item in each tuple is the index of the first course in that major in the course list
    // (the list of courses is organized alphabetically by major so the first 30 are all '2D Art and Design'...
    // and the next 30 are all 'Biology', etc., so the index of the first course of each major in the list
    // always increases incrementally by 30 as seen below)
    public List<(string, int)> majors = new List<(string, int)>{("2D Art and Design", 0), ("Agriculture", 30), 
    ("Biology", 60), ("Chemistry", 90), ("Combat and Weapon Use", 120), ("Computer Science", 150),
    ("Culinary and Food Studies", 180), ("Equestrian Studies", 210), ("Fashion", 240), ("Information and Library Science", 270),
    ("Literary Studies", 300), ("Magic Use", 330), ("Mathematics", 360), ("Philosophy", 390), ("Psychology", 420),
    ("Theater", 450), ("Writing and Journalism", 480)};

}