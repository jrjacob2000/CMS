(function () {

     //var app1 = angular.module("APP", ['kendo.directives', 'angularUtils.directives.dirPagination']);

    var webAPIurl = "/api/FundAPI/";

    angular.module('APP').service('FundService', function ($http) {

        this.GetFunds = function (id) {                  
            var request = $http.get(webAPIurl, { params: { "parentId": id } });
            return request;                     
        }

        //Create new record
        this.CreateBalanceFund = function (balFund) {
            var request = $http({
                method: "POST",
                url: webAPIurl,
                data: JSON.stringify(balFund)
            });
            return request;
        }
    })

    angular.module('APP').controller("fundController", function ($filter, $scope, $http, FundService) {

        //this event is being triggered from contributionController
        $scope.$on('refreshData', function (event, arg) {
            //$scope.receiver = 'got your ' + arg;
            LoadFundList();
        });

        $scope.selectedAction = {};
        $scope.Funds = [];
        LoadFundList();

        function LoadFundList() {
            FundService.GetFunds('')
             .then(function (response) {
                 $scope.Funds = response.data;
                 //set Default value for dropdown
                 $scope.SelectedBalanceFund = $scope.Funds[0];                 
             },
            function (errorPl) {
                $scope.ShowMessage('Failed in loading Funds: ' + errorPl.data, "error");
            })
        }

        $scope.FundModal = function (event, bal) {
            event.preventDefault();
            $scope.createBalanceFundPopup.center().open();
            $scope.selectedAction = bal;
        };

        $scope.createFund = function () {
            var acct = {
                Name: $scope.FundForm.Name,
                Category: $scope.selectedAction.Category,
                Type: $scope.selectedAction.AccountType
            };

            var parentAccount = {
                id: $scope.SelectedBalanceFund.Id,
                Name: $scope.SelectedBalanceFund.Name
            };
            
            var fund = {
                Parent: $scope.selectedAction.Category == 'FUNDACTIVITY' ? parentAccount : '',
                Account: acct,
                FundType: $scope.selectedAction.FundType
            };

            FundService.CreateBalanceFund(fund)
            .then(function (response) {
                LoadFundList();
                $scope.createBalanceFundPopup.close();
                //trigger the event refreshData in fundController
                $rootScope.$broadcast('reloadReference', 'message');
                $scope.ShowMessage('Saving successfull:', "success");                
            },
            function (errorPl) {
                $scope.createBalanceFundPopup.close();
                $scope.ShowMessage('Failed in loading Funds: ' + errorPl.data, "error");                
            })
        };

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