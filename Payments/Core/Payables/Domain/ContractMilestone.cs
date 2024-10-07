/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information holder                      *
*  Type     : ContractMilestone                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract milestone that is a payable object.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Contracts;

using Empiria.Payments.Payables.Data;

namespace Empiria.Payments.Payables {

  /// <summary>Represents a contract milestone that is a payable object.</summary>
  internal class ContractMilestone : Payable {

    #region Constructors and parsers

    internal ContractMilestone(Contract contract) : base(PayableType.ContractMilestone) {
      Assertion.Require(contract, nameof(contract));
      Assertion.Require(!contract.IsEmptyInstance,
                        "contract can not be the empty instance");

      Contract = contract;
    }

    private ContractMilestone(PayableType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new ContractMilestone Parse(int id) => ParseId<ContractMilestone>(id);

    static internal new ContractMilestone Parse(string UID) {
      return ParseKey<ContractMilestone>(UID);
    }

    static public new ContractMilestone Empty => ParseEmpty<ContractMilestone>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("PYM_PYB_CONTRACT_ID")]
    public Contract Contract {
      get; private set;
    }

    #endregion Properties

    #region Methods

    protected override void OnSave() {
      PayableData.WriteContractMilestone(this, base.ExtData.ToString());
    }

    #endregion Methods

  }  // class ContractMilestone

}  // namespace Empiria.Payments.Payables
