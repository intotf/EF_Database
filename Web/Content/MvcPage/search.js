(function ($, W) {
    var searchKey = 'search';

    function setSearch(param) {
        var array = [];
        for (var key in param) {
            array.push(key + '=' + encodeURI(param[key]));
        }
        document.cookie = searchKey
            + '='
            + array.join('&')
            + '; path='
            + location.pathname.replace(/\/index\/?$/, '');
    }

    function getSearchCookie() {
        var cookie = document.cookie;
        if (!cookie) {
            return null;
        }

        var cookies = cookie.split('; ');
        for (var i = 0; i < cookies.length; i++) {
            var kvs = cookies[i].split('=');
            if (kvs.shift() == searchKey) {
                return kvs.join('=');
            }
        }
        return null;
    }

    function getSearch() {
        var temp = {};
        var cookie = getSearchCookie();
        if (!cookie) {
            return temp;
        }

        var kvs = cookie.split('&');
        for (var i = 0; i < kvs.length; i++) {
            var kv = kvs[i].split('=');
            if (kv.length == 2) {
                temp[kv[0]] = decodeURI(kv[1]);
            }
        }
        return temp;
    }

    $(function () {
        $(document).on("click", "[field]", function () {
            var field = $(this).attr("field");
            var sort = $(this).attr("sort") == "asc" ? "desc" : "";
            var param = { orderby: $.trim(field + ' ' + sort) };

            var search = $.extend(getSearch(), param);
            setSearch(search);
            location.reload();
            if ($(".table .tdNoData").length > 0) {
                $(".tdNoData").attr("colspan", $(".table thead>tr>th").length).css({ "text-align": "center" });
            }
        });

        $("[data-search=search-form]").submit(function (e) {
            e.preventDefault();
            var param = {};
            $(this).find("input[name],select[name]").each(function () {
                param[$(this).attr("name")] = $(this).val();
            });

            var search = $.extend(getSearch(), param);
            setSearch(search);
            if (location.hash) {
                location.hash = '';
            } else {
                location.reload();
            }
            if ($(".table .tdNoData").length > 0) {
                $(".tdNoData").attr("colspan", $(".table thead>tr>th").length).css({ "text-align": "center" });
            }
        });

        $(document).on("change", "select[datapagesize]", function () {
            var param = { pageSize: $(this).val() };
            var search = $.extend(getSearch(), param);
            setSearch(search);
            location.reload();
            if ($(".table .tdNoData").length > 0) {
                $(".tdNoData").attr("colspan", $(".table thead>tr>th").length).css({ "text-align": "center" });
            }
        });

        $(document).on("click", "#cboSelect", function () {
            if ($(this).is(":checked") == true) {
                var strSelect = [];
                $("input[name=cbID]").each(function (index, ele) {
                    $(ele).prop("checked", true);
                    strSelect.push($(ele).val());
                });
                $("#hidd_CboSelect").val(strSelect.join(','));
            } else {
                $("input[name=cbID]").prop("checked", false);
                $("#hidd_CboSelect").val('');
            }
        });
    });

    W.showMask = function (jq) {
        $('<div class="mask-lay"><div><img src="/content/mvcpager/loading.gif"/></div></div>')
            .appendTo(jq.addClass("relative"))
            .fadeIn();
    };
})(jQuery, window);