/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Fields DTO                              *
*  Type     : ContractMilestoneFields                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Fields structure used for create and update contract milestone objects.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Contracts;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Fields structure used for create and update contract milestone objects.</summary>
  public class ContractMilestoneFields : PayableFields {

    public string ContractUID {
      get; set;
    }

    internal override void EnsureValid() {
      base.EnsureValid();

      Assertion.Require(ContractUID, "Necesito el contracto.");
      _ = Contract.Parse(ContractUID);
    }

  }  // class ContractMilestoneFields

}  // namespace Empiria.Payments.Payables.Adapters
