/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial cost object data                 Component : Data Layer                              *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Data Service                            *
*  Type     : FinancialCostObjetcData                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for financial cost object data.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Financial.CostObject.Data {

  /// <summary>Provides data access services for financial cost object data.</summary>
  static internal class FinancialCostObjectData {

    static internal FixedList<FinancialCostObject> GetCostObjects() {
      
     return BaseObject.GetFullList<FinancialCostObject>();
    }

    static internal FixedList<FinancialCostObject> GetByAreaCostObjects(int AreaId) {

      return BaseObject.GetFullList<FinancialCostObject>($"acct_org_unit_id = {AreaId}","Description");
    }

    static internal void Write(FinancialCostObject o) {
      var op = DataOperation.Parse("write_FMS_CostObject",
          o.Id, o.UID, o.CostType.Id, o.ExternalCode, o.Description,
          o.OrgUnit.Id, o.ExtData, o.Keywords, o.StartDate, o.EndDate,
          o.Id, o.PostedBy.Id, o.PostingTime,
          (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class FinancialCostObjectData

}  // namespace Empiria.Financial.CostObjects.Data
