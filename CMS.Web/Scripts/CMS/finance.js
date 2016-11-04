
(function () {



    var app = angular.module("APP", ['kendo.directives', 'angularUtils.directives.dirPagination']);

    var selectedId;
    var webAPIurl = "/api/TransactionAPI/";
    var webAPIurlbank = '/api/BankAPI/';
    var webAPIurlReferences = '/api/ReferenceAPI/';
        
    app.service('ReferencesService', function ($http) {
        this.GetAccountOptions = function(id)
        {
            var request = $http({
                method: "GET",
                url: webAPIurlReferences + id
            });
            return request;
        }
    })

    app.service('TransactionService', function ($http) {

        this.GetContribution = function (id) {
            var request = $http({
                method: "GET",
                url: webAPIurl + id
            });
            return request;
        }

        this.GetTransactionList = function (type,dateFilter,page) {
            var request = $http.get(webAPIurl, { params: {"type":type,"dateFilter":dateFilter, "page":page}});
            return request;
        }

        //Create new record
        this.CreateTransaction = function (transaction) {
            var request = $http({
                method: "POST",
                url: webAPIurl,
                data: JSON.stringify(transaction)
            });
            return request;
        }

        //Update record
        this.UpdateTransaction = function (transaction) {
            var request = $http({
                method: "PUT",
                url: webAPIurl,
                data: JSON.stringify(transaction)
            });
            return request;
        }

        //Delete record
        this.Delete = function (id) {
            var request = $http({
                method: "DELETE",
                url: webAPIurl + id
            });
            return request;
        }
        
    });

    app.service('BankService', function ($http) {

        this.GetBankActivities = function (id, dateFilter, page) {
            var request = $http.get(webAPIurlbank, { params: { "bankId": id, "dateFilter": dateFilter, "page": page } });
            return request;
        }

        this.GetBankInfos = function () {
            var request = $http.get(webAPIurlbank);
            return request;
        }

        //Create new record
        this.CreateBankAccount = function (bank) {
            var request = $http({
                method: "POST",
                url: webAPIurlbank,
                data: JSON.stringify(bank)
            });
            return request;
        }

        //Update record
        this.UpdateBankAccount = function (bank) {
            var request = $http({
                method: "PUT",
                url: webAPIurlbank,
                data: JSON.stringify(bank)
            });
            return request;
        }

        //Delete record
        this.DeleteBankAccount = function (id) {
            var request = $http({
                method: "DELETE",
                url: webAPIurlbank + id
            });
            return request;
        }
    });

    app.controller("financialController", function ($scope, $http) {
        $scope.init = function () {
            $('#myTabs a').click(function (e) {
                e.preventDefault()
                $(this).tab('show')
            });
        };
    });

    app.controller("contributionController", function ($rootScope, $scope, $http, $filter, TransactionService, ReferencesService) {
        
        //Events
        //this event is being triggered from bankcotroller and fundcontroller
        $scope.$on('reloadReference', function (event, arg) {
            LoadReferences();
        });


        $scope.Referencelabel = 'Env#\Particulars'
        $scope.Contributions = [];
        $scope.currentPage = 1;
        $scope.totalContributions = 0;
        $scope.totalBalance = 0;
        $scope.saving = false;
        $scope.showUpdatingAnimation = false;
        //PopulateReferences
        $scope.Funds = [];
        $scope.Banks = [];
        LoadReferences();
        //-------------------
        LoadContribution(1);


        function LoadReferences()
        {
            ReferencesService.GetAccountOptions(1)
            .then(function (response) {
                $scope.Funds = response.data;
            },
            function (errorPl) {
                $scope.ShowMessage('Failed in loading Contribution: ' + errorPl.data, "error");
            })

            ReferencesService.GetAccountOptions(3)
           .then(function (response) {
               $scope.Banks = response.data;
           },
           function (errorPl) {
               $scope.ShowMessage('Failed in loading Bank reference in contribution: ' + errorPl.data, "error");
           })
        }

        function LoadContribution(pageNumber) {
            var dateFilter = '';
            if($scope.SearchDate != null){
                dateFilter = $scope.SearchDate;
            };
            TransactionService.GetTransactionList(1, dateFilter, pageNumber)
             .then(function (response) {
                 $scope.Contributions = response.data.Items;
                 $scope.totalContributions = response.data.Count;
                 $scope.totalBalance = response.data.TotalBalance;
             },
            function (errorPl) {
                $scope.ShowMessage('Failed in loading Contribution: ' + errorPl.data, "error");
            })
        }

        $scope.search = function()
        {            
            LoadContribution($scope.currentPage);
        }

        $scope.pageChanged = function (newPage) {
            LoadContribution(newPage);
        };

        $scope.save = function (event) {
            event.preventDefault();
            $scope.saving = true;
            var Contribution = {
                Id: $scope.Contribution.Id,
                Reference: $scope.Contribution.Reference,
                Notes: $scope.Contribution.Notes,
                CreditAccount: $scope.Contribution.CreditAccount.Account,
                DebitAccount: $scope.Contribution.DebitAccount,
                Amount: $scope.Contribution.Amount,
                Date: $scope.Contribution.Date
            }
            
            var promisePost = TransactionService.CreateTransaction(Contribution);
            promisePost.then(function (pl) {                
                $scope.Contribution = {};
                LoadContribution($scope.currentPage);
                $scope.saving = false;

                //trigger the event refreshData in fundController
                $rootScope.$broadcast('refreshData', 'message');

                $scope.ShowMessage("Contribution save successfully", "success");               
            }, function (err) {
                $scope.ShowMessage('Error occured while saving: ' + err.data, "error");
                $scope.saving = false;
            });
        }
        
        $scope.openModal = function (event, contrb)
        {
            event.preventDefault();            
            $scope.updateContributionPopup.center().open();            
            $scope.updateContribForm = contrb;

            //set the default value of dropdown
            angular.forEach($scope.Funds, function (fund, key) {
                if (fund.Id == contrb.CreditAccount.Id)
                    $scope.updateContribForm.CreditAccount = fund;
            });

            angular.forEach($scope.Banks, function (bank, key) {
                if (bank.Id == contrb.DebitAccount.Id)
                    $scope.updateContribForm.DebitAccount = bank;
            });
            $scope.Banks

        };
        
        $scope.delete = function (event, id) {
            //event.preventDefault();
            TransactionService.Delete(id)
             .then(function (response) {
                 LoadContribution($scope.currentPage);

                 //trigger the event refreshData in fundController
                 $rootScope.$broadcast('refreshData', 'message');

                 $scope.ShowMessage("Deleted successfully", "success");
             },
            function (errorPl) {
                $scope.ShowMessage("Deletion failed:" + errorPl.data, "error");
            })
        };

        $scope.update = function (event) {
            event.preventDefault();
            $scope.showUpdatingAnimation = true;
           
            var Contribution = {
                Id: $scope.updateContribForm.Id,
                Reference: $scope.updateContribForm.Reference,
                Notes: $scope.updateContribForm.Notes,
                CreditAccount: $scope.updateContribForm.CreditAccount,
                DebitAccount: $scope.updateContribForm.DebitAccount,
                Amount: $scope.updateContribForm.Amount,
                Date: $scope.updateContribForm.Date
            }

            var promisePost = TransactionService.UpdateTransaction(Contribution);
            promisePost.then(function (pl) {
                LoadContribution($scope.currentPage);
                $scope.showUpdatingAnimation = false;
                $scope.updateContributionPopup.close();

                //trigger the event refreshData in fundController
                $rootScope.$broadcast('refreshData', 'message');

                $scope.ShowMessage("Contribution update successfully", "success");                
            }, function (err) {
                $scope.showUpdatingAnimation = false;
                $scope.updateContributionPopup.close();
                $scope.ShowMessage('Error occured while updating: ' + err.data, "error");                
            });
        }

        $scope.ShowMessage = function (message, messageType) {
            $scope.popupNotification = $('#msgNotification').kendoNotification({
                appendTo: "#msgContainer",
                autoHideAfter: 5000,
                templates: [{
                    type: "success",
                    template: '<div class="k-widget k-notification k-notification-success" style="display: block; opacity: 1;"><div class="k-notification-wrap"><span class="k-icon k-i-tick"></span>#= myMessage #</div></div> '
                }]
            }).data('kendoNotification');

            if (messageType == "success")
                $scope.popupNotification.show({ myMessage: message }, messageType);
            $scope.popupNotification.show(message, messageType);
        };

    });

    app.controller("expensesController", function ($rootScope,$filter, $scope, $http, TransactionService, ReferencesService) {
        
        //this event is being triggered from contributionController
        $scope.$on('reloadReference', function (event, arg) {
            LoadReferences();
        });

        $scope.Referencelabel = 'Pay to the order of';
        $scope.Expenses = [];
        $scope.currentPage = 1;
        $scope.totalRecords = 0;
        $scope.saving = false;
        $scope.showUpdatingAnimation = false;
        //Populate references
        $scope.ExpenseOptions = [];
        $scope.Banks = [];
        LoadReferences();
        LoadExpenseList(1);


        function LoadReferences() {
            ReferencesService.GetAccountOptions(2)
            .then(function (response) {
                $scope.ExpenseOptions = response.data;
            },
            function (errorPl) {
                $scope.ShowMessage('Failed in loading Expense Account Options 2: ' + errorPl.data, "error");
            })

            ReferencesService.GetAccountOptions(3)
           .then(function (response) {
               $scope.Banks = response.data;
           },
           function (errorPl) {
               $scope.ShowMessage('Failed in loading Expense Account Options 3: ' + errorPl.data, "error");
           })
        }

        function LoadExpenseList(pageNumber) {
            var dateFilter = '';
            if ($scope.SearchDate != null) {
                dateFilter = $scope.SearchDate;
            };
            TransactionService.GetTransactionList(2, dateFilter, pageNumber)
             .then(function (response) {
                 $scope.Expenses = response.data.Items;
                 $scope.totalRecords = response.data.Count;
             },
            function (errorPl) {
                $scope.ShowMessage('Failed in loading Expenses: ' + errorPl.data, "error");
            })
        }

        $scope.search = function () {
            LoadExpenseList($scope.currentPage);
        }

        $scope.pageChanged = function (newPage) {
            LoadExpenseList(newPage);
        };

        $scope.save = function (event) {
            event.preventDefault();
            $scope.saving = true;
            var Expense = {
                Id: $scope.Expense.Id,
                Reference: $scope.Expense.Reference,
                Notes: $scope.Expense.Notes,
                CreditAccount: $scope.Expense.CreditAccount,
                DebitAccount: $scope.Expense.DebitAccount.Account,
                Amount: $scope.Expense.Amount,
                Date: $scope.Expense.Date                
            }

            var promisePost = TransactionService.CreateTransaction(Expense);
            promisePost.then(function (pl) {
                $scope.Expense = {};                
                LoadExpenseList($scope.currentPage);
                $scope.saving = false;

                //trigger the event refreshData in fundController
                $rootScope.$broadcast('refreshData', 'message');

                $scope.ShowMessage("Expense save successfully", "success");
            }, function (err) {
                $scope.ShowMessage('Error occured while saving: ' + err.data, "error");
                $scope.saving = false;
            });
        }

        $scope.openModal = function (event, contrb) {
            event.preventDefault();
            $scope.updateExpensePopup.center().open();
            $scope.updateContribForm = contrb;
        };

        $scope.delete = function (event, id) {
            //event.preventDefault();
            TransactionService.Delete(id)
             .then(function (response) {
                 LoadExpenseList($scope.currentPage);

                 //trigger the event refreshData in fundController
                 $rootScope.$broadcast('refreshData', 'message');

                 $scope.ShowMessage("Deleted successfully", "success");
             },
            function (errorPl) {
                $scope.ShowMessage("Deletion failed:" + errorPl.data, "error");
            })
        };
        
        $scope.update = function (event) {
            event.preventDefault();
            $scope.showUpdatingAnimation = true;

            var Expense = {
                Id: $scope.updateContribForm.Id,
                Reference: $scope.updateContribForm.Reference,
                Notes: $scope.updateContribForm.Notes,
                TransactionType: $scope.transactionType,
                Account: $scope.updateContribForm.Account,
                Amount: $scope.updateContribForm.Amount,
                Date: $scope.updateContribForm.Date
            }

            var promisePost = TransactionService.UpdateTransaction(Expense);
            promisePost.then(function (pl) {
                LoadExpenseList($scope.currentPage);
                $scope.showUpdatingAnimation = false;
                $scope.updateExpensePopup.close();

                //trigger the event refreshData in fundController
                $rootScope.$broadcast('refreshData', 'message');

                $scope.ShowMessage("Expense update successfully", "success");
            }, function (err) {
                $scope.showUpdatingAnimation = false;
                $scope.updateExpensePopup.close();
                $scope.ShowMessage('Error occured while updating: ' + err.data, "error");
            });
        }

        $scope.ShowMessage = function (message, messageType) {
            $scope.popupNotification = $('#msgNotification').kendoNotification({
                appendTo: "#msgContainer",
                autoHideAfter: 5000,
                templates: [{
                    type: "success",
                    template: '<div class="k-widget k-notification k-notification-success" style="display: block; opacity: 1;"><div class="k-notification-wrap"><span class="k-icon k-i-tick"></span>#= myMessage #</div></div> '
                }]
            }).data('kendoNotification');

            if (messageType == "success")
                $scope.popupNotification.show({ myMessage: message }, messageType);
            $scope.popupNotification.show(message, messageType);
        };


    });
    
    app.controller("bankingController", function ($rootScope, $filter, $scope, $http, BankService, ReferencesService, TransactionService)
    {
        //this event is being triggered from contributionController and expenseController
        $scope.$on('refreshData', function (event, arg) {
            LoadBankActivities();
            LoadBankInfos();
        });

        $scope.SearchDate = $filter('date')(new Date(), 'MM/01/yyyy');;
        $scope.currentPage = 1;
        $scope.totalRecords = 0;
        $scope.totalBalance = 0;
        $scope.selectedBank = {};
        $scope.BankActivities = [];
        $scope.BankInfos = [];
        $scope.Banks = [];
        $scope.CurrentBank = {};
        Initialize();
               

        function Initialize() {
            
            ReferencesService.GetAccountOptions(3)
           .then(function (response) {
               $scope.Banks = response.data;
               $scope.selectedBank = $scope.Banks[0];
               LoadBankActivities(1);
               LoadBankInfos();
           },
           function (errorPl) {
               $scope.ShowMessage('Failed in loading Bank reference in Banking: ' + errorPl.data, "error");
           })
        }

        function LoadReferences()
        {
            ReferencesService.GetAccountOptions(3)
           .then(function (response) {
               $scope.Banks = response.data;
               $scope.selectedBank = $scope.Banks[0];
           },
           function (errorPl) {
               $scope.ShowMessage('Failed in loading Bank reference in Banking: ' + errorPl.data, "error");
           })
        }

        function LoadBankActivities(pageNumber) {
            var dateFilter = '';
            if ($scope.SearchDate != null) {
                dateFilter = $scope.SearchDate;
            };

            BankService.GetBankActivities($scope.selectedBank.Id, dateFilter, pageNumber)
             .then(function (response) {
                 $scope.BankActivities = response.data.Items;
                 $scope.totalRecords = response.data.Count;
                 $scope.totalBalance = response.data.TotalBalance;
             },
            function (errorPl) {
                $scope.ShowMessage('Failed in loading Deposits: ' + errorPl.data, "error");
            })
        }

        function LoadBankInfos()
        {
            BankService.GetBankInfos()
                .then(function (response) {
                $scope.BankInfos = response.data;               
            },
            function (errorPl) {
                $scope.ShowMessage('Failed in loading Deposits: ' + errorPl.data, "error");
            })
        }

        $scope.pageChanged = function (newPage) {            
            LoadBankActivities(newPage);
        };

        $scope.searchBankActivity = function () {
            LoadBankActivities(1);
        };

        //************************************
        //Tabs
        //************************************
        $scope.tab = 1;

        $scope.setTab = function (newValue) {
            $scope.tab = newValue;
        };

        $scope.showActivity = function (newValue, selectedBank) {
            $scope.selectedBank = selectedBank;
            LoadBankActivities(1);
            $scope.tab = newValue;
        };

        $scope.isSet = function (tabName) {
            return this.tab === tabName;
        };
        //************************************
        
        $scope.UpsertBankModal = function (event,bank) {          
            $scope.upsertBankPopup.center().open();
            $scope.BankForm = bank;
        };

        $scope.BankTransferModal = function (event, bank) {
            $scope.bankTransferPopup.center().open();
        };

        $scope.Save = function ()
        {
            var bank = $scope.BankForm;
          
            if (bank.Id == null) {
                BankService.CreateBankAccount(bank)
                .then(function (response) {
                    LoadBankInfos();

                    alert("Save successfully");
                    //trigger the event refreshData in fundController
                    $rootScope.$broadcast('reloadReference', 'message');
                    $scope.upsertBankPopup.close();
                },
                function (errorPl) {
                    $scope.ShowMessage('Failed in loading Deposits: ' + errorPl.data, "error");
                })
            }
            else
            {                
                BankService.UpdateBankAccount(bank)
               .then(function (response) {
                   LoadBankInfos();
                   LoadReferences();
                   alert("Save successfully");
                   //trigger the event refreshData in fundController
                   $rootScope.$broadcast('reloadReference', 'message');
                   $scope.upsertBankPopup.close();
               },
               function (errorPl) {
                   $scope.ShowMessage('Failed in loading Deposits: ' + errorPl.data, "error");
               })
            }
        };

        $scope.Delete = function (event,id)
        {
            BankService.DeleteBankAccount(id)
            .then(function (response) {
                LoadBankInfos();
                LoadReferences();
                alert("Deleted successfully");
                //trigger the event refreshData in fundController
                $rootScope.$broadcast('reloadReference', 'message');
            },
            function (errorPl) {
                alert(errorPl.data);
                $scope.ShowMessage('Failed in loading Deposits: ' + errorPl.data, "error");
            })
        };

        $scope.Transfer = function (event) {
            event.preventDefault();

            var transfer = {
                Id: '',
                Reference: '',
                Notes: 'Transfer',
                CreditAccount: $scope.TransferForm.CreditAccount,
                DebitAccount: $scope.TransferForm.DebitAccount,
                Amount: $scope.TransferForm.Amount,
                Date: $scope.TransferForm.TransactionDate
            }

            var promisePost = TransactionService.CreateTransaction(transfer);
            promisePost.then(function (pl) {
                alert('Success');
                LoadBankInfos();
                //$scope.saving = false;

                //trigger the event refreshData in fundController
                $rootScope.$broadcast('refreshData', 'message');

                
                $scope.bankTransferPopup.center().close();
                //$scope.ShowMessage("transfer successfully", "success");
            }, function (err) {
                alert(err.data);
                //$scope.ShowMessage('Error occured while saving: ' + err.data, "error");
                //$scope.saving = false;
            });
        }


        
    });

    app.controller("depositController", function ($filter, $scope, $http, BankService) {
        $scope.transactionType = 'DEPOSIT';
        $scope.Deposits = [];
        $scope.currentPage = 1;
        $scope.totalDeposits = 0;
        $scope.totalBalance = 0;
        $scope.saving = false;
        $scope.showUpdatingAnimation = false;
        LoadDepositList(1);

        function LoadDepositList(pageNumber) {
            var dateFilter = '';
            if ($scope.SearchDate != null) {
                dateFilter = $scope.SearchDate;
            };
            BankService.GetTransactionList($scope.transactionType, dateFilter, pageNumber)
             .then(function (response) {
                 $scope.Deposits = response.data.Items;
                 $scope.totalDeposits = response.data.Count;
                 $scope.totalBalance = response.data.TotalBalance;
             },
            function (errorPl) {
                $scope.ShowMessage('Failed in loading Deposits: ' + errorPl.data, "error");
            })
        }


        $scope.pageChanged = function (newPage) {
            LoadDepositList(newPage);
        };

        $scope.save = function (event) {
            event.preventDefault();
            $scope.saving = true;
            var Deposit = {
                Id: $scope.Deposit.Id,
                Reference: 'General Fund',//$scope.Deposit.Reference,
                Notes: $scope.Deposit.Notes,
                TransactionType: $scope.transactionType,
                Account: $scope.Deposit.Account,
                Amount: $scope.Deposit.Amount,
                Date: $scope.Deposit.Date
            }

            var promisePost = BankService.CreateTransaction(Deposit);
            promisePost.then(function (pl) {
                $scope.Deposit = {};
                LoadDepositList($scope.currentPage);
                $scope.saving = false;
                $scope.ShowMessage("Deposit save successfully", "success");
            }, function (err) {
                $scope.ShowMessage('Error occured while saving: ' + err.data, "error");
                $scope.saving = false;
            });
        }

        $scope.openModal = function (event, contrb) {
            event.preventDefault();
            $scope.updateDepositPopup.center().open();
            $scope.updateDepositForm = contrb;
        };

        $scope.delete = function (event, id) {
            //event.preventDefault();
            BankService.Delete(id)
             .then(function (response) {
                 LoadDepositList($scope.currentPage);
                 $scope.ShowMessage("Deleted successfully", "success");
             },
            function (errorPl) {
                $scope.ShowMessage("Deletion failed:" + errorPl.data, "error");
            })
        };

        $scope.update = function (event) {
            //event.preventDefault();
            $scope.showUpdatingAnimation = true;

            var Deposit = {
                Id: $scope.updateDepositForm.Id,
                Reference: $scope.updateDepositForm.Reference,
                Notes: $scope.updateDepositForm.Notes,
                TransactionType: $scope.transactionType,
                Account: $scope.updateDepositForm.Account,
                Amount: $scope.updateDepositForm.Amount,
                Date: $scope.updateDepositForm.Date,
                TransactionId : $scope.updateDepositForm.TransactionId
            }

            var promisePost = BankService.UpdateTransaction(Deposit);
            promisePost.then(function (pl) {
                LoadDepositList($scope.currentPage);
                $scope.showUpdatingAnimation = false;
                $scope.updateDepositPopup.close();
                $scope.ShowMessage("Deposit update successfully", "success");
            }, function (err) {
                $scope.showUpdatingAnimation = false;
                $scope.updateDepositPopup.close();
                $scope.ShowMessage('Error occured while updating: ' + err.data, "error");
            });
        }

        $scope.ShowMessage = function (message, messageType) {
            $scope.popupNotification = $('#msgNotification').kendoNotification({
                appendTo: "#msgContainer",
                autoHideAfter: 5000,
                templates: [{
                    type: "success",
                    template: '<div class="k-widget k-notification k-notification-success" style="display: block; opacity: 1;"><div class="k-notification-wrap"><span class="k-icon k-i-tick"></span>#= myMessage #</div></div> '
                }]
            }).data('kendoNotification');

            if (messageType == "success")
                $scope.popupNotification.show({ myMessage: message }, messageType);
            $scope.popupNotification.show(message, messageType);
        };


    });

  

   
})();

