/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Provisions Management                      Component : Data Layer                              *
*  Assembly : Empiria.Provisions.Core.dll                Pattern   : Data Services                           *
*  Type     : ProvisionData                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides provisions persistance services.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Provisions {

  /// <summary>Provides provisions persistance services.</summary>
  static internal class ProvisionData {

    static internal void WriteProvision(Provision o) {
      var op = DataOperation.Parse("write_fms_provision",
                  o.Id, o.UID, o.PayableEntity.GetEmpiriaType().Id, o.PayableEntity.Id,
                  o.PaymentOrder.Id, o.ExtData.ToString(), o.Keywords, o.PostingTime,
                  o.PostedBy.Id, (char) o.Status);


      DataWriter.Execute(op);
    }

  }  // class ProvisionData

}  // namespace Empiria.Provisions
