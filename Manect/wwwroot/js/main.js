function ClickStageButton(Element, StageId) {
    if ($('#foreground').is(':visible')) {
        HideForm();  
    }
    else {
        var stage = {
            Id: Number(StageId)
        }
        SendStage(stage);

        var top = $('.' + Element).offset().top;
        var left = $('.' + Element).offset().left;

        $('#editStageForm').offset({ top: top });
        $('#project-step').css('marginLeft', left);
        $('#foreground').show();
    }
}

function ClickSaveStageButton(StageId) {
    var stage = {
        Id: Number(StageId)
    }
    SendStage(stage);
    HideForm();  
}

function HideForm() {
    $('#editStageForm').offset({ top: 0 });
    //$('#foreground').css('opacity', 0);
    $('#foreground').hide();
    $("#project-step").empty();
}

function SendStage(stage) {
    $.ajax({
        url: '/Project/GetStage',
        type: 'POST',
        cache: false,
        async: true,
        dataType: "html",
        data: { stage: stage },
        success: function (result) {
            $('#project-step').html(result);

        },
        error: function () {
            alert("AAAAAAAAAAAAAAAAAAAAAAAAAAAA...");
            $('#editStageForm').css('box-shadow', '');
        }
    });
}




!function (t) {
    function e(e) {
        for (var o, u, l = e[0], r = e[1], s = e[2], a = 0, f = []; a < l.length; a++)
            u = l[a],
                Object.prototype.hasOwnProperty.call(i, u) && i[u] && f.push(i[u][0]),
                i[u] = 0;
        for (o in r)
            Object.prototype.hasOwnProperty.call(r, o) && (t[o] = r[o]);
        for (p && p(e); f.length;)
            f.shift()();
        return c.push.apply(c, s || []),
            n()
    }
    function n() {
        for (var t, e = 0; e < c.length; e++) {
            for (var n = c[e], o = !0, l = 1; l < n.length; l++) {
                var r = n[l];
                0 !== i[r] && (o = !1)
            }
            o && (c.splice(e--, 1),
                t = u(u.s = n[0]))
        }
        return t
    }
    var o = {}
        , i = {
            0: 0
        }
        , c = [];
    function u(e) {
        if (o[e])
            return o[e].exports;
        var n = o[e] = {
            i: e,
            l: !1,
            exports: {}
        };
        return t[e].call(n.exports, n, n.exports, u),
            n.l = !0,
            n.exports
    }
    u.m = t,
        u.c = o,
        u.d = function (t, e, n) {
            u.o(t, e) || Object.defineProperty(t, e, {
                enumerable: !0,
                get: n
            })
        }
        ,
        u.r = function (t) {
            "undefined" != typeof Symbol && Symbol.toStringTag && Object.defineProperty(t, Symbol.toStringTag, {
                value: "Module"
            }),
                Object.defineProperty(t, "__esModule", {
                    value: !0
                })
        }
        ,
        u.t = function (t, e) {
            if (1 & e && (t = u(t)),
                8 & e)
                return t;
            if (4 & e && "object" == typeof t && t && t.__esModule)
                return t;
            var n = Object.create(null);
            if (u.r(n),
                Object.defineProperty(n, "default", {
                    enumerable: !0,
                    value: t
                }),
                2 & e && "string" != typeof t)
                for (var o in t)
                    u.d(n, o, function (e) {
                        return t[e]
                    }
                        .bind(null, o));
            return n
        }
        ,
        u.n = function (t) {
            var e = t && t.__esModule ? function () {
                return t.default
            }
                : function () {
                    return t
                }
                ;
            return u.d(e, "a", e),
                e
        }
        ,
        u.o = function (t, e) {
            return Object.prototype.hasOwnProperty.call(t, e)
        }
        ,
        u.p = "/";
    var l = window.webpackJsonp = window.webpackJsonp || []
        , r = l.push.bind(l);
    l.push = e,
        l = l.slice();
    for (var s = 0; s < l.length; s++)
        e(l[s]);
    var p = r;
    c.push([2, 1]),
        n()
}([, , function (t, e, n) {
    "use strict";
    n.r(e);
    n(3),
        n(4),
        n(5),
        n(6),
        n(7),
        n(9)
}
    , function (t, e, n) {
        (function (t) {
            !function (t) {
                "use strict";
                t(".checkbox label").on("click", (function (e) {
                    t(this).closest("li").toggleClass("checked")
                }
                ))
            }(t)
        }
        ).call(this, n(0))
    }
    , function (t, e, n) {
        (function (t) {
            !function (t) {
                "use strict";
                t(".select-box").on("click", (function (e) {
                    e.preventDefault(),
                        t(this).siblings(".select-options").is(":visible") ? t(".select-options").hide() : (t(".select-options").hide(),
                            t(this).siblings(".select-options").css("display", "flex"),
                            e.stopPropagation())
                }
                )),
                    t(".select-options").on("click", ".option", (function () {
                        t(this).closest(".select").find(".select-box span").html(t(this).find("span.name").text()),
                            t(".select-options").hide()
                    }
                    )),
                    t(document).on("click", (function (e) {
                        t(".option").is(e.target) || t(".select-options").hide()
                    }
                    ))
            }(t)
        }
        ).call(this, n(0))
    }
    , function (t, e, n) {
        (function (t) {
            !function (t) {
                "use strict";
                function e() {
                    t(".menu .module").animate({
                        opacity: 0
                    }, 300, (function () {
                        history.pushState("", document.title, window.location.pathname),
                            t(".menu, .menu .overlay, .menu .module").hide()
                    }
                    ))
                }
                t(".open-menu").on("click", (function (e) {
                    e.preventDefault(),
                        function (e) {
                            t(".menu, .menu .overlay").show(),
                                t(e).css("display", "flex").animate({
                                    opacity: 1
                                }, 300)
                        }(t(this).attr("href")),
                        e.stopPropagation()
                }
                )),
                    t(".close-menu").on("click", (function (t) {
                        t.preventDefault(),
                            e()
                    }
                    )),
                    t("body").keydown((function (t) {
                        27 == t.keyCode && e()
                    }
                    ))
            }(t)
        }
        ).call(this, n(0))
    }
    , function (t, e, n) {
        (function (t) {
            !function (t) {
                "use strict";
                var e = decodeURI(window.location.hash.replace("#", ""));
                function n(e) {
                    t(".popup .overlay").fadeIn(300, (function () {
                        t("html").css({
                            "overflow-y": "hidden",
                            "overflow-x": "hidden"
                        }),
                            t(".popup, .popup .overlay").show(),
                            t(e).css("display", "flex").animate({
                                opacity: 1
                            }, 300)
                    }
                    ))
                }
                function o() {
                    t(".popup .module").animate({
                        opacity: 0
                    }, 300, (function () {
                        history.pushState("", document.title, window.location.pathname),
                            t(".popup, .popup .overlay, .popup .module").hide(),
                            t(".popup .overlay").fadeOut(300),
                            t("html").css("overflow-y", "visible")
                    }
                    ))
                }
                t(".open-popup").on("click", (function (e) {
                    e.preventDefault(),
                        n(t(this).attr("href")),
                        e.stopPropagation()
                }
                )),
                    t.each(["stages"], (function (t, o) {
                        e == o.toString() && n("#" + e)
                    }
                    )),
                    t(".close-popup").on("click", (function (t) {
                        t.preventDefault(),
                            o()
                    }
                    )),
                    t("body").keydown((function (t) {
                        27 == t.keyCode && o()
                    }
                    ))
            }(t)
        }
        ).call(this, n(0))
    }
    , function (t, e, n) {
        var o = n(1)
            , i = n(8);
        "string" == typeof (i = i.__esModule ? i.default : i) && (i = [[t.i, i, ""]]);
        var c = {
            insert: "head",
            singleton: !1
        };
        o(i, c);
        t.exports = i.locals || {}
    }
    , function (t, e, n) { }
    , function (t, e, n) {
        var o = n(1)
            , i = n(10);
        "string" == typeof (i = i.__esModule ? i.default : i) && (i = [[t.i, i, ""]]);
        var c = {
            insert: "head",
            singleton: !1
        };
        o(i, c);
        t.exports = i.locals || {}
    }
    , function (t, e, n) { }
]);