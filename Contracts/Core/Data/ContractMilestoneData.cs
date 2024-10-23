/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Milestone Management             Component : Data Layer                              *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Data service                            *
*  Type     : ContractData                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for contract milestone instances.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Data;

namespace Empiria.Contracts.Data {

  /// <summary>Provides data read and write methods for contract milestone instances.</summary>
  static internal class ContractMilestoneData {

    #region Methods

    static internal FixedList<ContractMilestone> getMilestone(string filter, string sortBy) {
      var sql = "select * from fms_milestone ";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" where {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" order by {sortBy}";
      }

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<ContractMilestone>(dataOperation);
    }

    static internal void WriteMilestone(ContractMilestone o, string extensionData) {
      var op = DataOperation.Parse("write_Contract_Milestone",
                     o.Id, o.UID, o.Contract.Id, o.PaymentNumber, o.PaymentDate,
                     o.ManagedByOrgUnit.Id, extensionData, o.Keywords,
                     o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class ContractMilestoneData

}  // namespace Empiria.Contracts.Data
