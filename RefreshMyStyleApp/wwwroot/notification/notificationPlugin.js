(function ($) {
    // ****** Add .notification.css ******
    $.fn.NotificationSetup = function (options) {
        /*
          Declaration : $("#noti_Container").NotificationSetup({
                    List: objCollectionList
          });
       */
        var defaultSettings = $.extend({
            BeforeSeenColor: "#2E467C",
            AfterSeenColor: "#ccc"
        }, options);
        $(".Noti_Button").css({
            "background": defaultSettings.BeforeSeenColor
        });
        var parentId = $(this).attr("id");
        if ($.trim(parentId) != "" && parentId.length > 0) {
            $("#" + parentId).append("<div class='Noti_Counter'></div>" +
                "<div class='Noti_Button'></div>" +
                "<div class='Notifications'>" +
                "<h3>Notifications (<span class='notiCounterOnHead'>0</span>)</h3>" +
                "<div class='NotificationItems'>" +
                "</div>" +
                "<div class='SeeAll'><a href='#'>See All</a></div>" +
                "</div>");

            $('#' + parentId + ' .Noti_Counter')
                .css({ opacity: 0 })
                .text(0)
                .css({ top: '-10px' })
                .animate({ top: '-2px', opacity: 1 }, 500);

            $('#' + parentId + ' .Noti_Button').click(function () {
                $('#' + parentId + ' .Notifications').fadeToggle('fast', 'linear', function () {
                    if ($('#' + parentId + ' .Notifications').is(':hidden')) {
                        $('#' + parentId + ' .Noti_Button').css('background-color', defaultSettings.AfterSeenColor);
                    }
                    else $('#' + parentId + ' .Noti_Button').css('background-color', defaultSettings.BeforeSeenColor);
                });
                $('#' + parentId + ' .Noti_Counter').fadeOut('slow');
                return false;
            });
            $(document).click(function () {
                $('#' + parentId + ' .Notifications').hide();
                if ($('#' + parentId + ' .Noti_Counter').is(':hidden')) {
                    $('#' + parentId + ' .Noti_Button').css('background-color', defaultSettings.AfterSeenColor);
                }
            });
            $('#' + parentId + ' .Notifications').click(function () {
                return false;
            });

            $("#" + parentId).css({
                position: "relative"
            });
        }
    };
    $.fn.NotificationCount = function (options) {
        /*
          Declaration : $("#myComboId").NotificationCount({
                    NotificationList: [],
                    NotiFromPropName: "",
                    ListTitlePropName: "",
                    ListBodyPropName: "",
                    ControllerName: "Notifications",
                    ActionName: "AllNotifications"
          });
       */
        var defaultSettings = $.extend({
            NotificationList: [],
            NotiFromPropName: "",
            ListTitlePropName: "",
            ListBodyPropName: "",
            ControllerName: "Notifications",
            ActionName: "AllNotifications"
        }, options);
        var parentId = $(this).attr("id");
        if ($.trim(parentId) != "" && parentId.length > 0) {
            $("#" + parentId + " .Notifications .SeeAll").click(function () {
                window.open('../' + defaultSettings.ControllerName + '/' + defaultSettings.ActionName + '', '_blank');
            });

            var totalUnReadNoti = defaultSettings.NotificationList.filter(x => x.isRead == false).length;
            $('#' + parentId + ' .Noti_Counter').text(totalUnReadNoti);
            $('#' + parentId + ' .notiCounterOnHead').text(totalUnReadNoti);
            if (defaultSettings.NotificationList.length > 0) {
                $.map(defaultSettings.NotificationList, function (item) {
                    var className = item.isRead ? "" : " SingleNotiDivUnReadColor";
                    var sNotiFromPropName = $.trim(defaultSettings.NotiFromPropName) == "" ? "" : item[LowerFirstLetter(defaultSettings.NotiFromPropName)];
                    $("#" + parentId + " .NotificationItems").append("<div class='SingleNotiDiv" + className + "' notiId=" + item.notiId + ">" +
                        "<h4 class='NotiFromPropName'>" + sNotiFromPropName + "</h4>" +
                        "<h5 class='NotificationTitle'>" + item[LowerFirstLetter(defaultSettings.ListTitlePropName)] + "</h5>" +
                            "<div class='NotificationBody'>" + item[LowerFirstLetter(defaultSettings.ListBodyPropName)] + "</div>" +
                        "<div class='NofiCreatedDate'>" + item.createdDateSt + "</div>" +
                        "</div>");
                    $("#" + parentId + " .NotificationItems .SingleNotiDiv[notiId=" + item.notiId + "]").click(function () {
                        if ($.trim(item.url) != "") {
                            window.location.href = item.url;
                        }
                    });
                });
            }
        }
    };
}(jQuery));

function LowerFirstLetter(value) {
    return value.charAt(0).toLowerCase() + value.slice(1);
}

