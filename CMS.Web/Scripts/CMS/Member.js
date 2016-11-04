(function () {

    var app = angular.module("APP", ['kendo.directives'])
    var selectedId;
    var webAPIurl = "/api/memberAPI/";

    app.service('MemberService', function ($http) {

        this.GetMember = function (id) {
            var request = $http({
                method: "GET",
                url: webAPIurl + id
            });
            return request;
        }

        this.GetMemberList = function () {
            var request = $http({
                method: "GET",
                url: webAPIurl
            });
            return request;
        }

        //Create new record
        this.CreateMember = function (Member) {
            var request = $http({
                method: "POST",
                url: webAPIurl,
                data: JSON.stringify(Member)
            });
            return request;
        }

        //Update record
        this.UpdateMember = function (Member) {
            var request = $http({
                method: "PUT",
                url: webAPIurl,
                data: JSON.stringify(Member)
            });
            return request;
        }

    });

    app.controller("membershipController", function ($filter, $scope, $http, MemberService) {

        var initLoad = true;
        $scope.Members = {};
        $scope.Member = {};
        $scope.IsNewMember = false;
        $scope.saving = false;
        $scope.popupNotification = {}
        LoadMemberList();

        function LoadMemberList() {

            $scope.loading = true;
            MemberService.GetMemberList()
                .then(function (response) {
                    $scope.Members = response.data;
                    if (initLoad) {
                        $scope.Member = $scope.Members[0];
                        selectedId = $scope.Member.Id;
                        initLoad = false;
                    }
                    else {
                        var result = $filter('filter')($scope.Members, { Id: selectedId })[0];
                        $scope.Member = result;
                    }
                    $scope.loading = false;
                },
                 function (errorPl) {
                     //ShowMessage('failed in loading Employees' + errorPl.data,"error");
                     $scope.loading = false;
                     alert('failed in loading Members ' + errorPl.data);
                 })

        }

        $scope.ReLoadMembers = function () {
            LoadMemberList();
        };

        $scope.isSelected = function (id) {
            return selectedId === id;
            $scope.IsNewMember = false;
        }

        $scope.select = function ($event, id) {
            selectedId = id;
            var result = $filter('filter')($scope.Members, { Id: id })[0];
            $scope.Member = result;
        };


        $scope.edit = function (edit) {
            var result = $filter('filter')($scope.Members, { Id: selectedId })[0];
            $scope.MemberForm = result;
        }

        $scope.toolbarOptions = {
            items: [
                { type: "button", id: "btnNew", text: "New", spriteCssClass: "k-icon k-i-plus" },

                { type: "button", id: "btnEdit", text: "Edit", spriteCssClass: "k-icon k-i-pencil" }
            ],
            click: function (e) {
                if (e.target.text() == "New") {
                    $scope.IsNewMember = true;
                    $scope.win2.title("Create Member");
                    $scope.win2.center().open();
                    $scope.MemberForm = {};
                }
                if (e.target.text() == "Edit") {
                    $scope.IsNewMember = false;
                    $scope.win2.title("Update Member");
                    $scope.win2.center().open();
                    $scope.$apply(function () { $scope.edit(); });
                }
            }
        };

        function onClose(e) {
            alert(e);
        }
    });

    app.controller("createMemberController", function ($filter, $scope, $http, MemberService) {
        $scope.save = function (event) {
            event.preventDefault();
            if ($scope.validator.validate()) {
                $scope.saving = true;
                var Member = {
                    Id: $scope.MemberForm.Id,
                    FirstName: $scope.MemberForm.FirstName,
                    MiddleName: $scope.MemberForm.MiddleName,
                    LastName: $scope.MemberForm.LastName,
                    Age: $scope.MemberForm.Age,
                    Gender: $scope.MemberForm.Gender,
                    Birthday: $scope.MemberForm.Birthday,
                    MobilePhone: $scope.MemberForm.MobilePhone,
                    LandLine: $scope.MemberForm.LandLine,
                    Address: $scope.MemberForm.Address,
                    MaritalStatus: $scope.MemberForm.MaritalStatus,
                    NameOfSpouse: $scope.MemberForm.NameOfSpouse,
                    SpouseContact: $scope.MemberForm.SpouseContact,
                    ChildrenCount: $scope.MemberForm.ChildrenCount,
                    MemberStatus: $scope.MemberForm.MemberStatus,
                    BaptizedDate: $scope.MemberForm.BaptizedDate,
                    BaptizedPlace: $scope.MemberForm.BaptizedPlace,
                    BaptizedMinister: $scope.MemberForm.BaptizedMinister,
                    BelongsToGroups: $scope.MemberForm.BelongsToGroups,
                    Positions: $scope.MemberForm.Positions
                };

                if ($scope.IsNewMember) {
                    var promisePost = MemberService.CreateMember(Member);
                    promisePost.then(function (pl) {
                        var id = pl.data.Id;
                        $scope.saving = false;//turn off the saving progress
                        $scope.ReLoadMembers();
                        $scope.select(null, id);
                        $scope.win2.close();
                        $scope.ShowMessage("Member save successfully", "success");
                    }, function (err) {
                        $scope.ShowMessage('Error occured while creating new member: ' + err.data, "error");
                        $scope.saving = false;
                        $scope.win2.close();
                    })

                }
                else {
                    var promisePost = MemberService.UpdateMember(Member);
                    promisePost.then(function (pl) {
                        $scope.saving = false;
                        $scope.ReLoadMembers();
                        $scope.select(null, Member.Id);
                        $scope.win2.close();
                        $scope.ShowMessage("Member save successfully", "success");
                    }, function (err) {
                        $scope.saving = false;
                        $scope.win2.close();
                        $scope.ShowMessage('Error occured while updating: ' + err.data, "error");
                    })

                }
            }

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