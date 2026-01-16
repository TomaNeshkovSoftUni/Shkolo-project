Credentials: 
1) Admin - username: "admin"; pass: "adminpassword"
2) User - username: "user1"; pass: "userpassword"

!! update the connection string in ShkoloContext.cs if your SQL instance isn't Server=..
!! Initialize: Run Update-Database in the Package Manager Console.

JSON file must be an array. example structure:

```
 {
     "FirstName": "Daniela",
     "LastName": "Marinova",
     "Class": "10B",
     "Grades": [
         {
             "SubjectName": "History",
             "Score": 5
         },
         {
             "SubjectName": "Biology",
             "Score": 6
         }
     ]
 },
```

*_this is a school project_
