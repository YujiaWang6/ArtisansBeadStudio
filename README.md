# ArtisansBeadStudio
This is the collaboration project with Bead Keychain Design Platform and Art Gallary. 

The Art Gallary project is a Web app for "Image upload" like Gallery in our Phones, or Google Photos. In this app, the user can have the space where they can check out the list of Albums which stores the images and further, can check image details and can edit or delete the image details which is basically CRUD Operations for Image table.

The Bead Keychain Design Platform is the Web app could replace the bead design board in the real world. It allows users to store their beads online, including uploaded the image of beads to visualize exact look of that specific bead. They can CRUD the bead on the website. There is the list of bead colour which allows user to see all the beads in that specific colour. Also, users can CRUD the colour on the website. Besides the beads themselves, there is keychain option which allow user to create their own keychain online! They can add/remove the bead to specific keychain, in that specific keychain, they can see the pictures of all the beads in that specific keychain. They can also CRUD the keychain to manage their design.

## Functions
There are several extra functions for this collaboration project.
- It allows users to associate/unassociate styles (from Art Gallary) to a keychain (from Bead Keychain Design Platform).
- Also, inside the style page, users can see all the keychains link to that specific style.
- Beside that, the authetication is used for differentiating the Admin user and the Guest users. The Admin user can manage all the keychains created online, while the Guest users can only manage the keychains created by their own.

## Build Status 👾
The current status is that there is CRUD Functionality for all the Tables in our project.
- CRUD (Create, Read, Update & Delete) for Bead.✅
- CRUD (Create, Read, Update & Delete) for BeadColour.✅
- CRUD (Create, Read, Update & Delete) for Image.✅
- CRUD (Create, Read, Update & Delete) for Keychain.✅
- CRUD (Create, Read, Update & Delete) for Style.✅
- CR (Create, Read) for Album.

## Tech/Framework 💻
- ASP.NET MVC: Utilize the Model-View-Controller (MVC) architectural pattern for building the web application.
- Entity Framework: Use Entity Framework with a Code-First approach for managing the database schema and migrations.
- Web API: Create Web API controllers to handle data retrieval and manipulation, facilitating collaboration between the projects.
- Razor Views: Design Razor views (.cshtml files) to generate dynamic HTML content for displaying data to users.
- Collaboration: Establish a connection between the "styles" table from the art gallery project and the "keychain" table from the bead-making project, allowing seamless exploration of keychains by style and vice versa.
- Migration: Implement database migrations to manage and update the database schema as the project evolves.

## Installation
1. Clone the repository from GitHub:
   ```
   git clone https://github.com/YujiaWang6/ArtisansBeadStudio.git
   ```
2. Update the local database:
   - Create a ```App_Data``` folder inside your local ArtisansBeadStudio folder
   - Run update the database command
   ```
   Tools > Nuget Package Manager > Package Manage Console > Update-Database
   ```
   - Check if the database is created:
   ```
   View > SQL Server Object Explorer > MSSQLLocalDb >...
   ```
3. Set up the Admin and Guest role
   - Register an account
   - View > SQL Server Object Explorer
   - Create 'Guest', 'Admin' entries in AspNetRoles
   - Copy UserID from AspNetUsers table
   - Create entry between Guest Role x User Id, Admin Role x User Id in AspNetUserRoles bridging table
4. Now you are ready to use the website and create some data by your own!

