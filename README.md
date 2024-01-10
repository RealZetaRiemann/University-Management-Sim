# University Management Simulation game in Unity

## Stats:
<ul>
<li>Academics</li>
<li>Prestige</li>
<li>GP (money)</li>
</ul>

## Basic Gameplay:

<p>Gameplay is split up into rounds called “semesters” of play. Each semester is split up into three phases:</p>
<ol>
  <li>Hire Staff</li>
  <li>Admit Students</li>
  <li>Choose Courses</li>
</ol>

## Winning Condition(s):
<p>Academics > 100 && Prestige > 100 && GP > 1000000</p>

## Losing Conditions:
<p>Academics < 0 || Prestige < 0 || GP < 0</p>

## 1: Hiring Staff

<p>During the hiring phase you may hire up to three professors:</p>

<ul>
  <li>Professors are free to hire and last forever</li>
  <li>Upkeep cost: 30GP per class they teach each semester (max 3)</li>
  <li>Each professor knows how to teach five classes in their major specialty, at least one of which is always a required course</li>
</ul>

<p>Once you have at least one professor hired, every future list of potential hires will include at least one professor whose major specialty is one that you already have</p>

<p>When you hire a professor whose major specialty is one that you don’t already have, gain Academics + 5</p>
<p>When you hire the 5th professor with the same specialty as four other already-hired professors, gain Academics + 15</p>

## 2: Admitting Students

<p>During the admission phase, you may admit up to X students where X is the number of students applying:</p>
<p>Number of students applying = 20 + (3 * Prestige)</p>

<p>Prestige will increase or decrease based on how many applicants are accepted each semester:</p>
<ul>
  <li>Acceptance rate > 75% lowers Prestige by 2</li>
  <li>Acceptance rate > 50% lowers Prestige by 1</li>
  <li>Acceptance rate < 50% raises Prestige by 1</li>
  <li>Acceptance rate < 25% raises Prestige by 2</li>
</ul>

<p>Students pay 5GP per course they take each semester (max 4 courses)</p>


## 3: Choose which courses will run

<p>A course can be offered more than once during a semester as long as there’s a professor available to teach it</p>

<p>If a student finishes their senior year (8th semester) without all their graduation requirements, they will drop out instead of graduating and lower Prestige by 1</p>

<p>Graduation requirements include all of the student’s required courses for their major and at least 10 other classes taken from their major</p>
