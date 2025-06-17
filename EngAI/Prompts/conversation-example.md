Conversation Context
--------------------
Topic: Explain Code to Non-Developers
Conversation Context: Explain the functionality and code structure of a new feature to a non-technical Project Manager
CEFR Level: B2

**Conversation Example**

*Instructor:*
<!--Explaining the role-play context-->
-You are a Software Engineer who has just completed a new feature for a software application. Your task is to explain the functionality and the underlying code to a non-technical Project Manager. Your goal is to make the technical aspects understandable and highlight the benefits of the new feature.
<!--Listing out role-play tasks to the student-->>
```
{
    "tasks":[
        {
            "task":"Explain the new feature's purpose clearly.",
            "isCompleted":false
        },
        {
            "task":"Describe the code structure of in simple terms.",
            "isCompleted":false
        },
        {
            "task":"Highlight benefits for the team and users.",
            "isCompleted":false
        }
    ]
}
```
<!--From now on the instructor will role-play themselves as a Project Manager-->
*Instructor:*
-Hello! Can you walk me through the new feature you just implemented?

*Student:*
-Of course! This new feature allows the user to login to our app using email and password, or using third-party authentication like Google or Facebook.

*Instructor:*
```
{
    "tasks":[
        {
            "task":"Explain the new feature's purpose clearly.",
            "isCompleted":true
        },
        {
            "task":"Describe the code structure in simple terms.",
            "isCompleted":false
        },
        {
            "task":"Highlight benefits for the team and users.",
            "isCompleted":false
        }
    ]
}
```
-That sounds great! Can you explain how the code is structured to support this feature?

*Student:*
-The code is structured in a modular way. We have implemented a separate module for user authentication, which handle the logic for both email/password and third-party logins. The module has its own set of functions that manage the login process, error handling, and user session management.

*Instructor:*
```
{
    "tasks":[
        {
            "task":"Explain the new feature's purpose clearly.",
            "isCompleted":true
        },
        {
            "task":"Describe the code structure in simple terms.",
            "isCompleted":true
        },
        {
            "task":"Highlight benefits for the team and users.",
            "isCompleted":false
        }
    ]
}
```
-Understandable! How does this benefit our team and the users?

*Student:*
-This feature greatly improves user experience in general by helping the user secure their data in the app, but also providing a more convenient way to login. For the team, it simplifies the authentication process and reduces the amount of code we need to maintain, since we are using verified libraries for third-party authentication.

*Instructor:*
```
{
    "tasks":[
        {
            "task":"Explain the new feature's purpose clearly.",
            "taskCompleted":true
        },
        {
            "task":"Describe the code structure in simple terms.",
            "taskCompleted":true
        },
        {
            "task":"Highlight benefits for the team and users.",
            "taskCompleted":true
        }
    ]
}
```
-That's good to know! It sounds like a valuable addition to our application. Thank you for the clear explanation!
<!--Student can end the role-play here-->
