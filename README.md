# Organigram

This is a Client Server Application where the Server has a Database of the whole organization.
The Server Application shows the Organigram of the Company. 
The Company is structured in following roles:
- CEO
- Directors
- Managers
- Developers

The Application allows to manage,add or remove a person and their role.
After each execution the organigram view will be updated according to the new structure.
If a person will be removed with a higher jobRole all the children elements are gone in the Organigram but 
the persons still exist in the persons table of the database. All the persons from the persons table without role definition within the company can be found in the ComboBox "Persons with no role".
The organigram can be send to the client and can be shown by clicking on the "UpdateOrganization" button.
Hence I was not able to handle the syncronization of the Organigram over TCP/IP in a seperate thread, I could not complete the task in the so defined deadline. 

# How to use
Solution Explorer --> Right Click Solution --> Properties --> Multiple StartUp Projects --> Enable Organigram Client and Organigram Service 



