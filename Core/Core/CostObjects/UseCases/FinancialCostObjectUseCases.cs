/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Cost Object                        Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases interactor                  *
*  Type     : FinancialCostObjectUseCases                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for retrieve and update financial cost object.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial.CostObject.Adapters;
using Empiria.Parties;

namespace Empiria.Financial.CostObject.UseCases {

  /// <summary>Provides use cases for retrieve and update financial cost objects.</summary>
  public class FinancialCostObjectUseCases : UseCase {

    #region Constructors and parsers

    protected FinancialCostObjectUseCases() {
      // no-op
    }

    static public FinancialCostObjectUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FinancialCostObjectUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public  FixedList<FinancialCostObjectDto> GetCostObjects() {

      FixedList<FinancialCostObject> costObject = FinancialCostObject.GetList("COBJ_STATUS <> 'X'", "COBJ_DESCRIPTION");

      return FinancialCostObjectMapper.Map(costObject);
    }

    public FixedList<FinancialCostObjectDto> SearchCostObjectsByOrgUnit(FinancialCostObjectEntry entry) {

      var party = Party.Parse(entry.requestedByUID);
      FixedList <FinancialCostObject> costObject = FinancialCostObject.GetList($"COBJ_ORG_UNIT_ID = {party.Id} AND COBJ_STATUS <> 'X'", "COBJ_DESCRIPTION");

      return FinancialCostObjectMapper.Map(costObject);
    }


    public FinancialCostObjectDto GetByUID(string uid) {
      var costObject = FinancialCostObject.Parse(uid);

      return FinancialCostObjectMapper.Map(costObject);
    }

    public FinancialCostObjectDto CreateCostObject(FinancialCostObjectEntry entry) {
      Assertion.Require(entry, nameof(entry));

      var costObject = FinancialCostObject.Create(entry);
      costObject.Save();

      return FinancialCostObjectMapper.Map(costObject);
    }

    public FinancialCostObjectDto UpdateCostObject(string uid, FinancialCostObjectEntry entry) {
      var costObject = FinancialCostObject.Parse(uid);

      costObject.Update(entry);
      costObject.Save();

      return FinancialCostObjectMapper.Map(costObject);
    }

    public void DeleteCostObject(string uid) {
      Assertion.Require(uid, nameof(uid));

      var costObject = FinancialCostObject.Parse(uid);

      costObject.Delete();
      costObject.Save();
    }
    #endregion Use cases

  }  // class FinancialCostObjectUseCases

}  // namespace Empiria.Financial.CostObject.UseCases
