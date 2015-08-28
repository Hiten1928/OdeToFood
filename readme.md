OdeToFood
Web application that allows you to register restaurant, add reviews to it, book tables for the specific time
---------------------------------------------------------
Modeles included:
- OdeToFood.Web
- OdeToFood.Data
- OdeToFood.Constacts
- OdeToFood.Tests
---------------------------------------------------------
OdeToFood.Web
Contains the main features and being responsible for the user experience. It contains controllers, views, IoC, Authentication, javascripts, styles.
Configured with SQL Express

OdeToFood.Data
Responsible to the data access usinf Entity Framework. It contains models, context, repositories and data access abstractions

OdeToFood.Contracts
Contains interfaces

OdeToFood.Tests
Contains unit tests (nUnit)
---------------------------------------------------------
Examples of use (for OdeToFood.Data):
public class MyClass
{
   private readonly DataRepository _dataRepository;
   public MyClass(DataRepository dataRepository)
   {
      _dataRepository = dataRepository;
   }
   
   public void PlayWithEntities(int id)
   {
      MyEntity entity1 = _dataContext.MyEntity.Get(id);

      MyEntity entity2 = _dataContext.MyEntity.Delete(id);

      MyEntity entity3 = new MyEntity();
      _dataContext.Add(entity3);
   }
}