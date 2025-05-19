/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Projects Management                        Component : Test cases                              *
*  Assembly : Empiria.Projects.Core.Tests.dll            Pattern   : Testing variables                       *
*  Type     : TestingVariables                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FinancialProject type.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Tests.Financial.Projects {

  internal class TestingVariables {

    static internal T TryGetRandomNonEmpty<T>() where T: BaseObject {
      var objects = BaseObject.GetFullList<T>()
                              .FindAll(x => !x.IsEmptyInstance);

      if (objects.Count == 0) {
        return null;
      }

      int randomIndex = EmpiriaMath.GetRandom(0, objects.Count - 1);

      return objects[randomIndex];
    }

  }  // class TestingVariables

}  // namespace Empiria.Tests.Financial.Projects
