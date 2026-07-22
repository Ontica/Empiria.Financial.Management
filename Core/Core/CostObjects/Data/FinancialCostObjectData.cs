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

using Empiria.Financial.CostObject;

namespace Empiria.Financial.CostObject.Data {

  /// <summary>Provides data access services for financial cost object data.</summary>
  static internal class FinancialCostObjectData {

    static internal FixedList<FinancialCostObject> GetActiveCostObjects() {
      var sql = "SELECT * FROM FMS_COST_OBJECTS " +
                "WHERE COBJ_STATUS <> 'X' " +
                "AND COBJ_START_DATE <= SYSDATE AND COBJ_END_DATE >= SYSDATE " +
                "ORDER BY COBJ_EXTERNAL_NO";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FinancialCostObject>(op);
    }

    static internal FinancialCostObject TryGetByExternalCode(string externalCode) {

      var sql = "SELECT * FROM FMS_COST_OBJECTS " +
          "WHERE COBJ_STATUS = 'A' " +
          $"AND COBJ_EXTERNAL_NO = '{externalCode}'" +
          "ORDER BY COBJ_EXTERNAL_NO";


      var op = DataOperation.Parse(sql, externalCode);

      return DataReader.GetPlainObject<FinancialCostObject>(op, null);
    }

    static internal void Write(FinancialCostObject o) {
      var op = DataOperation.Parse("write_FMS_CostObject",
          o.Id, o.UID, o.CostObjectType, o.ExternalCode, o.Description,
          o.ExtData, o.Keywords, o.StartDate, o.EndDate,
          o.HistoricId, o.PostedById, o.PostingTime,
          (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class FinancialCostObjectData

}  // namespace Empiria.Financial.CostObjects.Data
