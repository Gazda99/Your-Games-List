# YGL.API


Most of the endpoints have to be called with bearer token provided in Header.
```
Authorization: bearer {token}
```
If type of response field is written inside brackets:
[Type]
 it means this field is an array of that type.
# API endpoints
Most of the GET requests can be done in two ways:

By providing which ids exactly we want to get, by sending GET requests:
```
/api/{version}/{controller}/{ids}
```
where ids can be specified like: (multiple ids)
```
/1,2,10,6,3,20-40,50-70 
```
Those requests will get response:
```
{
"data":[
	{
	"requestedItem":{
	  }
	}
  ],
  "amount": int
}
```

 ---
By providing additional query parameters available on specific endpoint, with (or without) skip and take.
Skip - how many items to skip. Default is 0.
Take - how many items to be requested. Default and Max is 100.

```
/api/{version}/{controller}?queryParam=exampleName&skip=10&take=100
```

Those requests will get response:
```
{
"data":[
	{
	"requestedItem":{
	  }
	}
  ],
  "amount": int,
  "skipped": int,
  "nextAt": int
}
```

---

### Company<sub>(GET)</sub> 
```
/companies
```
| Query param|Type|
|-|-|
|companyName|string|

Response:
"gameMode":
| Field |Type|
|-|-|
|id|int|
|description|string|
|name|string|
|country|int|


---

### GameMode<sub>(GET)</sub>
```
/gameModes
```
| Query param|Type|
|-|-|
|gameModeName|string|

Response:
| Field |Type|
|-|-|
|id|int|
|name|string|
---

### Genre<sub>(GET)</sub>
```
/genres
```
| Query param|Type|
|-|-|
|genreName|string|

Response:
| Field |Type|
|-|-|
|id|int|
|name|string|
---

### Platform<sub>(GET)</sub>

```
/platforms
```
| Query param|Type|
|-|-|
|platformName|string|

Response:
| Field |Type|
|-|-|
|id|int|
|abbr|string|
|name|string|
---
### Player Perspective<sub>(GET)</sub>
```
/playerPerspectives
```
| Query param|Type|
|-|-|
|perspectiveName|string|

Response:
| Field |Type|
|-|-|
|id|int|
|name|string|
---

### User<sub>(GET)</sub>
```
/users
```
| Query param|Type|
|-|-|
|username|string|
|showInactive|bool|

Response:
| Field |Type|
|-|-|
|id|long|
|gender|byte|
|birthYear|short|
|country|short|
|createdAt|DateTime|
|about|string|
|rank|string|
|experience|string|
|itemStatus|bool|
|listsOfGames|[long]|
|groups|[long]|
|friends|[long]|
---


# Identity endpoints
No support for multiple ids requesting and additional query parameters like take and skip.

They all starts with:
```
/identity/{version}/
```


---

### Login <sup>i</sup><sub>(POST)</sub>
- Used for obtaining authorization token.
```
/login
```

Post data:
| Field |Type|
|-|-|
|username|string|
|password|string|


Response:
| Field |Type|
|-|-|
|userId|long|
|jwtToken|string|
|refreshToken|string|
---


### Register <sup>i</sup><sub>(POST)</sub>
 - Registers new user.

```
/register
```
Post data:
| Field |Type|
|-|-|
|email|string (email format)|
|username|string|
|password|string|


Response:
| Field |Type|
|-|-|
|isSuccess|bool|
---

### Refresh <sup>i</sup><sub>(POST)</sub>
- Used for refreshing the authorization token without logging again.
```
/refresh
```
Post data:
| Field |Type|
|-|-|
|jwtToken|string|
|refreshToken|string|


Response:
| Field |Type|
|-|-|
|jwtToken|string|
|refreshToken|string|
---

### Email Confirmation Resend Email <sup>i</sup><sub>(POST)</sub>
- Sends a mail with account confirmation to provided email. (such email is already sent when creating account with /register endpoint).
```
/emailconfirmationresendemail
```

Post data:
| Field |Type|
|-|-|
|email|string|


Response:
| Field |Type|
|-|-|
|isSuccess|bool|
---

### Email Confirmation <sup>i</sup><sub>(GET)</sub>
 - For confirming account creation.
```
/emailconfirmation
```

| Query param|Type|
|-|-|
|url|string|

Response:
| Field |Type|
|-|-|
|id|int|
|name|string|
---

### Password Reset Send Email<sup>i</sup><sub>(POST)</sub>
 - Sends mail with link to continue with password reset process.
 
```
/passwordresetsendemail
```
Post data:
| Field|Type|
|-|-|
|email|string|

Response:
| Field |Type|
|-|-|
|isSuccess|bool|
---
### Password Reset Confirmation <sup>i</sup><sub>(GET)</sub>
 - Confirms the will to change password and gets the password reset token needed for password reset.
 
```
/passwordresetconfirmation
```

| Query param|Type|
|-|-|
|url|string|

Response:
| Field |Type|
|-|-|
|passwordResetToken|string|
---
### Password Reset <sup>i</sup><sub>(POST)</sub>
 - Resets the password
 
```
/passwordreset
```
Post data:
| Field|Type|
|-|-|
|resetPasswordToken|string|
|newPassword|string|

Response:
| Field |Type|
|-|-|
|isSuccess|bool|
---
# Health Checks
Api provides ability to check status of specific services.
```
/health
```
Reponse:
```
{
  "status": ,
  "checks": [
    {
      "status": "",
      "component": "",
      "description": ""
    }
  ],
  "duration": "",
  "date": ""
}
```

---
# Errors
All errors have same structure:
```
{
  "errorMessages": [
    string
  ],
  "errorCodes": [
    int
  ]
}

```

---
<sup>i</sup> endpoint does not need bearer token passed

<sup>ii </sup> GET endpoint does not support multiple ids

<sup>iii </sup> GET endpoint does not support take and skip feature

