# USER GUIDE — EduPlatform

EduPlatform is a web application for managing courses, quizzes, user progress and automated scoring.
This guide explains how to use the system step-by-step.

## Table of Contents

Getting Started

User Registration & Login

Main Navigation

Courses Module

Quizzes Module

Taking a Quiz

Viewing Results

Admin Features

Troubleshooting

## Getting Started

Once you launch the application (locally or on a server), open:

https://localhost:5001


You will see the homepage with navigation at the top.

## User Registration & Login
▶ Register

Click Register in the navbar

Fill in:

Email

Password

Confirm password

Click Create account

After that, you can log in.

▶ Login

Click Login

Enter your email and password

You will be redirected to the homepage as an authenticated user

## Main Navigation

Navigation menu contains:

Menu Item	Description
Home	Main landing page
Courses	List of available courses
Quizzes	All quizzes in the system
Results	Your past quiz attempts with scores
Login / Register	Authentication links
Logout (if logged in)	Ends your session
  
## Courses Module
▶ View all courses

Navigate to Courses
You will see a list with:

Course title

Description

Actions (Details / Edit / Delete — only for admins)

▶ Course details

Shows full description and all related quizzes.

## Quizzes Module

Navigate to Quizzes to view all quizzes.

You will see:

Field	Meaning
Course	Which course quiz belongs to
Title	Quiz name
Time Limit	Duration in minutes
Actions	Edit / Delete / Start
▶ Start quiz

Press Start button next to the quiz you want — the system opens the quiz interface.

## Taking a Quiz

The quiz page includes:

Progress bar

Question steps (1 question per screen)

Radio buttons with answer choices

Navigation buttons:

Next

Back

Submit (on last question)

▶ How to answer

Select one answer for each question

Move through the quiz using Next / Back

Press Submit to finish

## Viewing Results

After submission, you are redirected to:

/TakeQuiz/Result/<yourResultId>


Results page shows:

Quiz title

Total score

Count of correct answers

Every question with:

Your answer

Correct answer

Explanation (if implemented)

You can always return to this page using Results menu.

🛠 Admin Features

Admins (roles: Admin) can:

✔ Create courses
✔ Edit and delete courses
✔ Create quizzes
✔ Add questions & answers
✔ Manage users (if Identity UI enabled)

These actions appear automatically when logged in as an admin.

## Troubleshooting
Issue	Solution
Can’t submit quiz (400 BadRequest)	Ensure AntiForgery token is enabled or tests disable it properly
Quiz shows no questions	Add questions under the quiz in the admin panel
Can’t log in	Reset password or recreate user
Test project failing	Verify mock database seed & correct form fields

## Summary

This User Guide covers everything required to use EduPlatform:

Register & log in

Browse courses and quizzes

Take quizzes with progress navigation

View results

Perform admin actions

If you need a PDF version or want me to generate screenshots, just say — I can auto-generate them.
