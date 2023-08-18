# ArtisansBeadStudio
This is the collaboration project with Bead Keychain Design Platform and Art Gallary. 

The Art Gallary project is a Web app for "Image upload" like Gallery in our Phones, or Google Photos. In this app, the user can have the space where they can check out the list of Albums which stores the images and further, can check image details and can edit or delete the image details which is basically CRUD Operations for Image table.

The Bead Keychain Design Platform is the Web app could replace the bead design board in the real world. It allows users to store their beads online, including uploaded the image of beads to visualize exact look of that specific bead. They can CRUD the bead on the website. There is the list of bead colour which allows user to see all the beads in that specific colour. Also, users can CRUD the colour on the website. Besides the beads themselves, there is keychain option which allow user to create their own keychain online! They can add/remove the bead to specific keychain, in that specific keychain, they can see the pictures of all the beads in that specific keychain. They can also CRUD the keychain to manage their design.

## Functions
There are several extra functions for this collaboration project.
- It allows users to associate/unassociate styles (from Art Gallary) to a keychain (from Bead Keychain Design Platform).
- Also, inside the style page, users can see all the keychains link to that specific style.
- Beside that, the authetication is used for differentiating the Admin user and the Guest users. The Admin user can manage all the keychains created online, while the Guest users can only manage the keychains created by their own.  

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

