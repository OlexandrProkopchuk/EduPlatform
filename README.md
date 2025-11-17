# EduPlatform — Educational Quiz Platform  
**ASP.NET Core MVC | Entity Framework Core | Bootstrap 5 | xUnit Integration Tests**

---

## Project Overview

**EduPlatform** is an educational web platform built with **ASP.NET Core MVC**.  
It allows users to:

- Create and manage **courses**
- Add and configure **quizzes**
- Add **questions** and **answer options**
- Pass quizzes through an interactive step-by-step interface
- View quiz results and scoring
- Run **full integration tests** (xUnit + TestServer + SQLite InMemory)

This project was developed as part of a laboratory work assignment in .NET/C#.

---

## Features

### Courses
- Full CRUD
- Each course can contain multiple quizzes

### Quizzes
- Title, time limit, attached course
- CRUD functionality

### Questions & Answers
- Unlimited number of questions per quiz  
- Each question has several answer options  
- Support for marking correct answers  

### Taking a Quiz
- Questions are shown **step-by-step**
- Navigation: *Previous / Next / Submit*
- Progress bar
- Form submission format:

QuizId=1
Answers[1]=3
Answers[5]=14


### Quiz Results
- Correct answer calculation via `QuizService`
- Storing quiz attempts
- Displaying score and selected answers  

---

## Technologies Used

| Technology | Purpose |
|-----------|---------|
| **ASP.NET Core MVC (.NET 8)** | main application framework |
| **Entity Framework Core** | data access |
| **SQL Server / SQLite InMemory** | persistent + test database |
| **Bootstrap 5** | UI and layout |
| **Identity** | authentication |
| **xUnit + WebApplicationFactory** | integration tests |

---

## Project Structure

EduPlatformSolution/
├── Controllers/
├── Data/
│ ├── AppDbContext.cs
│ ├── Models/
│ ├── ViewModels/
├── Services/
├── Views/
├── EduPlatform.IntegrationTests/
│ ├── Infrastructure/
│ ├── Pages/
├── wwwroot/
├── Program.cs
├── appsettings.json
└── README.md


## Author

Developed as part of a laboratory assignment in .NET / C#.
Students: Prokopchuk Oleksandr, Polina Grishko of group IPZ-33/8 
