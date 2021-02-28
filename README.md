## Bereau de' Change Web App
This is a simple web app to FX funds transfer from local currency. Built with asp.net core 3.1 (C#) and JSON datastore using a custom built and generic DataContext manager which simulates a real db instance with .json files.
Presentation is built with MVC Core, HTML, jQuery and Bootstrap.

- Daily transfer limit of 500.00
- Payment is flexible done with fake response to simulate a real API call
- During transfer, if you get 'Debit error', click the button until you get success
- If the FX Credit fails, retry another transaction. Success if a result of 1 from a random set of 1 to 3.
- Dashboard can onlyshow a max of 6 transactions
- Admin dashboard can only show max of 20 records for transfers and users

### Features and Highlights
- Create account
- Login
- Transfer funds (Local to FX)
- Transfer history
- Display realtime exchange rate
- Debit with fake Paystack response
- Credit with fake Swift repsonse
- Background processing of transfer to show multiple states
- Admin panel
	- View transfers
	- View users

### Setup
On start of the application all neccessary store files are being created to /DataStore/{schema}.json

### State Management
Caching is carried out with Static objects references to enhance performances. On user logout, all cache items are cleared

### User Access
**Administrator**
Email: admin@gmail.com
Password: 12345

**Users**
First signup from the homepage
Login with your email address and a static password **123456**