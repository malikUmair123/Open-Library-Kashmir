$(document).ready(function () {

    // Load countries on page load
    $.getJSON("https://secure.geonames.org/countryInfoJSON", { username: "olkorg" })
        .done(function (data) {
            console.log("Countries data:", data);
            var items = "<option value=''>Select Country</option>";
            $.each(data.geonames, function (index, country) {
                items += "<option value='" + country.countryName + "' data-geonameid='" + country.geonameId + "'>" + country.countryName + "</option>";
            });
            items += "<option value='Other'>Other</option>"; // Add this line for "Other" option
            $("#country-dropdown").html(items);

            // Check if citydefault value exists in the model
            var selectedcountry = $("#countrydefault").val();
            if (selectedcountry) {
                $("#country-dropdown").val(selectedcountry).trigger("change"); // Trigger change event
            } else
            {
                // Set default country to India
                $("#country-dropdown").val("India").trigger("change"); // Trigger change event
            }

        })
        .fail(function (jqxhr, textStatus, error) {
            var err = textStatus + ", " + error;
            console.error("Request Failed: " + err);
        });

    $("#country-dropdown").change(function () {

            if ($(this).val() === "Other") {
                $("#country-textbox").show();
                $("#state-dropdown").val("Other").trigger("change"); // Trigger change event
            } else {
                $("#country-textbox").hide().val("");

                var countrygeonameid = $(this).find("option:selected").data("geonameid"); // Retrieve the geoname ID from the selected option

                // Load states for India on page load
                $.getJSON("https://secure.geonames.org/childrenJSON", { geonameId: countrygeonameid, username: "olkorg" })
                    .done(function (data) {
                        console.log("States data:", data);
                        var items = "<option value=''>Select State</option>";
                        $.each(data.geonames, function (index, state) {
                            items += "<option value='" + state.name + "' data-geonameid='" + state.geonameId + "'>" + state.name + "</option>";
                        });
                        items += "<option value='Other'>Other</option>"; // Add this line for "Other" option
                        $("#state-dropdown").html(items);

                        // Check if citydefault value exists in the model
                        var selectedstate = $("#statedefault").val();
                        if (selectedstate) {
                            $("#state-dropdown").val(selectedstate).trigger("change"); // Trigger change event
                        }
                        else {
                            // Set default state to Jammu and Kashmir
                            $("#state-dropdown").val("Jammu and Kashmir").trigger("change"); // Trigger change event
                        }

                    })
                    .fail(function (jqxhr, textStatus, error) {
                        var err = textStatus + ", " + error;
                        console.error("Request Failed: " + err);
                    });
        }
    });


    // Load cities/district for India on page load

    $("#state-dropdown").change(function () {

        if ($(this).val() === "Other") {
            $("#state-textbox").show();
            $("#city-dropdown").val("Other").trigger("change"); // Trigger change event
        } else {
            $("#state-textbox").hide().val(""); 

            var stategeonameid = $(this).find("option:selected").data("geonameid"); // Retrieve the geoname ID from the selected option

            $.getJSON("https://secure.geonames.org/childrenJSON", { geonameId: stategeonameid, username: "olkorg" })
                .done(function (data) {

                    console.log("Cities data:", data);
                    var items = "<option value=''>Select City/District</option>";
                    $.each(data.geonames, function (index, city) {
                        items += "<option value='" + city.name + "' data-geonameid='" + city.geonameId + "'>" + city.name + "</option>";
                    });
                    items += "<option value='Other'>Other</option>"; // Add this line for "Other" option
                    $("#city-dropdown").html(items);

                    // Check if citydefault value exists in the model
                    var selectedcity = $("#citydefault").val();
                    if (selectedcity) {
                        $("#city-dropdown").val(selectedcity).trigger("change"); // Trigger change event
                    }
                })
                .fail(function (jqxhr, textStatus, error) {
                    var err = textStatus + ", " + error;
                    console.error("Request Failed: " + err);
                });
        }
    });

    // Load tehsils
    $("#city-dropdown").change(function () {

        if ($(this).val() === "Other") {
            $("#city-textbox").show();
            $("#child-location-dropdown").val("Other").trigger("change"); // Trigger change event
        } else {
            $("#city-textbox").hide().val(""); 

            var districtId = $(this).find("option:selected").data("geonameid"); // Retrieve the geoname ID from the selected option
            $.getJSON("https://secure.geonames.org/childrenJSON", { geonameId: districtId, username: "olkorg" })
                .done(function (data) {
                    console.log("District locations data:", data);
                    var items = "<option value=''>Select Block/Tehsil</option>";
                    $.each(data.geonames, function (index, location) {
                        items += "<option value='" + location.name + "' data-geonameid='" + location.geonameId + "'>" + location.name + "</option>";
                    });
                    items += "<option value='Other'>Other</option>"; // Add this line for "Other" option
                    $("#child-location-dropdown").html(items);
                    // Check if citydefault value exists in the model
                    var selectedtehsil = $("#tehsildefault").val();
                    if (selectedtehsil) {
                        $("#child-location-dropdown").val(selectedtehsil).trigger("change"); // Trigger change event
                    }
                })
                .fail(function (jqxhr, textStatus, error) {
                    var err = textStatus + ", " + error;
                    console.error("Request Failed: " + err);
                });
        }
    });

    // Load villages
    $("#child-location-dropdown").change(function () {

        if ($(this).val() === "Other") {
            $("#child-location-textbox").show();
            $("#subchild-location-dropdown").val("Other").trigger("change"); // Trigger change event
            $("#postal-code-dropdown").val("Other").trigger("change"); // Trigger change event
        } else {
            $("#child-location-textbox").hide().val(""); 

            var tehsilId = $(this).find("option:selected").data("geonameid"); // Retrieve the geoname ID from the selected option
            $.getJSON("https://secure.geonames.org/childrenJSON", { geonameId: tehsilId, username: "olkorg" })
                .done(function (data) {
                    console.log("Villages locations data:", data);

                    var items = "<option value=''>Select Street/Village</option>";
                    $.each(data.geonames, function (index, location) {
                        items += "<option value='" + location.name + "' data-geonameid='" + location.geonameId + "'>" + location.name + "</option>";
                    });
                    items += "<option value='Other'>Other</option>"; // Add this line for "Other" option
                    $("#subchild-location-dropdown").html(items);

                    // Check if citydefault value exists in the model
                    var selectedvillage = $("#villagedefault").val();
                    if (selectedvillage) {
                        $("#subchild-location-dropdown").val(selectedvillage).trigger("change"); // Trigger change event
                    }

                    $.getJSON("https://secure.geonames.org/hierarchyJSON", {
                        geonameId: tehsilId,
                        username: "olkorg"
                    })
                        .done(function (data) {
                            console.log("Hierarchy JSON data:", data);

                            // Extract latitude and longitude of the last location in the hierarchy
                            var lastGeoname = data.geonames.slice(-1)[0]; // Get the last item in the array
                            var latitude = lastGeoname.lat;
                            var longitude = lastGeoname.lng;

                            // Make Json request to find nearby postal codes
                            $.getJSON("https://secure.geonames.org/findNearbyPostalCodesJSON", { lat: latitude, lng: longitude, username: "olkorg" })
                                .done(function (postalData) {
                                    console.log("Postal code data:", postalData);
                                    var items = "<option value=''>Select Pin Code</option>";
                                    $.each(postalData.postalCodes, function (index, location) {
                                        items += "<option value='" + location.postalCode + "'>" + location.placeName + " - " + location.postalCode + "</option>";
                                    });
                                    items += "<option value='Other'>Other</option>"; // Add this line for "Other" option
                                    $("#postal-code-dropdown").html(items);

                                    // Check if citydefault value exists in the model
                                    var selectedpin = $("#pindefault").val();
                                    if (selectedpin) {
                                        $("#postal-code-dropdown").val(selectedpin).trigger("change"); // Trigger change event
                                    }
                                })
                                .fail(function (jqxhr, textStatus, error) {
                                    var err = textStatus + ", " + error;
                                    console.error("Request Failed: " + err);
                                });

                            // Output latitude and longitude
                            console.log("Latitude:", latitude);
                            console.log("Longitude:", longitude);
                        })
                        .fail(function (jqxhr, textStatus, error) {
                            var err = textStatus + ", " + error;
                            console.error("Request Failed: " + err);
                        });

                })
                .fail(function (jqxhr, textStatus, error) {
                    var err = textStatus + ", " + error;
                    console.error("Request Failed: " + err);
                });
        }
    });

    // Load villages
    $("#subchild-location-dropdown").change(function () {

        if ($(this).val() === "Other") {
            $("#subchild-location-textbox").show();
        } else {
            $("#subchild-location-textbox").hide().val(""); 
        }
    });

    // Load villages
    $("#postal-code-dropdown").change(function () {

        if ($(this).val() === "Other") {
            $("#postal-code-textbox").show();
        } else {
            $("#postal-code-textbox").hide().val(""); 
        }
    });

});