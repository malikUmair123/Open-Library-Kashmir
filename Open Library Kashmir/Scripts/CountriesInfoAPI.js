$(document).ready(function () {
    // Load countries on page load
    //$.getJSON("https://secure.geonames.org/countryInfoJSON", { username: "olkorg" })
    //    .done(function (data) {
    //        console.log("Countries data:", data);
    //        var items = "<option value=''>Select Country</option>";
    //        $.each(data.geonames, function (index, country) {
    //            items += "<option value='" + country.geonameId + "'>" + country.countryName + "</option>";
    //        });
    //        $("#country-dropdown").html(items);

    //        //// Set default country to India
    //        //$("#country-dropdown").val("IN").trigger("change"); // Trigger change event
    //    })
    //    .fail(function (jqxhr, textStatus, error) {
    //        var err = textStatus + ", " + error;
    //        console.error("Request Failed: " + err);
    //    });

    //$("#country-dropdown").change(function () {
    //    var countryCode = $(this).val();
    //    $.getJSON("https://secure.geonames.org/childrenJSON", { geonameId: countryCode, username: "olkorg" })
    //        .done(function (data) {
    //            console.log("States data:", data);
    //            var items = "<option value=''>Select State</option>";
    //            $.each(data.geonames, function (index, state) {
    //                items += "<option value='" + state.geonameId + "'>" + state.name + "</option>";
    //            });
    //            $("#state-dropdown").html(items);
    //        })
    //        .fail(function (jqxhr, textStatus, error) {
    //            var err = textStatus + ", " + error;
    //            console.error("Request Failed: " + err);
    //        });
    //});

    //$("#state-dropdown").change(function () {
    //    var stateId = $(this).val();
    //    $.getJSON("https://secure.geonames.org/childrenJSON", { geonameId: stateId, username: "olkorg" })
    //        .done(function (data) {
    //            console.log("Cities data:", data);
    //            var items = "<option value=''>Select City</option>";
    //            $.each(data.geonames, function (index, city) {
    //                items += "<option value='" + city.geonameId + "'>" + city.name + "</option>";
    //            });
    //            $("#city-dropdown").html(items);
    //        })
    //        .fail(function (jqxhr, textStatus, error) {
    //            var err = textStatus + ", " + error;
    //            console.error("Request Failed: " + err);
    //        });
    //});



    //Set Default Values else use generic one above


    // Load countries on page load
    $.getJSON("https://secure.geonames.org/countryInfoJSON", { username: "olkorg" })
        .done(function (data) {
            console.log("Countries data:", data);
            var items = "<option value=''>Select Country</option>";
            $.each(data.geonames, function (index, country) {
                items += "<option value='" + country.countryCode + "'>" + country.countryName + "</option>";
            });
            $("#country-dropdown").html(items);

            // Set default country to India
            $("#country-dropdown").val("IN").trigger("change"); // Trigger change event
        })
        .fail(function (jqxhr, textStatus, error) {
            var err = textStatus + ", " + error;
            console.error("Request Failed: " + err);
        });

    // Load states for India on page load
    $.getJSON("https://secure.geonames.org/childrenJSON", { geonameId: 1269750, username: "olkorg" })
        .done(function (data) {
            console.log("States data:", data);
            var items = "<option value=''>Select State</option>";
            $.each(data.geonames, function (index, state) {
                items += "<option value='" + state.name + "'>" + state.name + "</option>";
            });
            $("#state-dropdown").html(items);

            // Set default state to Jammu and Kashmir
            $("#state-dropdown").val("Jammu and Kashmir").trigger("change"); // Trigger change event
        })
        .fail(function (jqxhr, textStatus, error) {
            var err = textStatus + ", " + error;
            console.error("Request Failed: " + err);
        });

    // Load cities/district for India on page load

       $.getJSON("https://secure.geonames.org/childrenJSON", { geonameId: 1269320, username: "olkorg" })
           .done(function (data) {

               console.log("Cities data:", data);
               var items = "<option value=''>Select City/District</option>";
          $.each(data.geonames, function (index, city) {
                items += "<option value='" + city.name + "' data-geonameid='" + city.geonameId +  "'>" + city.name + "</option>";
            });
            $("#city-dropdown").html(items);
        })
        .fail(function (jqxhr, textStatus, error) {
            var err = textStatus + ", " + error;
            console.error("Request Failed: " + err);
        });


    // Load tehsils
    $("#city-dropdown").change(function () {
        var districtId = $(this).find("option:selected").data("geonameid"); // Retrieve the geoname ID from the selected option
        $.getJSON("https://secure.geonames.org/childrenJSON", { geonameId: districtId, username: "olkorg" })
            .done(function (data) {
                console.log("District locations data:", data);
                var items = "<option value=''>Select Block/Tehsil</option>";
                $.each(data.geonames, function (index, location) {
                    items += "<option value='" + location.name + "' data-geonameid='" + location.geonameId + "'>" + location.name + "</option>";
                });
                $("#child-location-dropdown").html(items);
            })
            .fail(function (jqxhr, textStatus, error) {
                var err = textStatus + ", " + error;
                console.error("Request Failed: " + err);
            });
    });

    // Load villages
    $("#child-location-dropdown").change(function () {
        var tehsilId = $(this).find("option:selected").data("geonameid"); // Retrieve the geoname ID from the selected option
        $.getJSON("https://secure.geonames.org/childrenJSON", { geonameId: tehsilId, username: "olkorg" })
            .done(function (data) {
                console.log("Villages locations data:", data);

                var items = "<option value=''>Select Street/Village</option>";
                $.each(data.geonames, function (index, location) {
                    items += "<option value='" + location.name + "' data-geonameid='" + location.geonameId +  "'>" + location.name + "</option>";
                });
                $("#subchild-location-dropdown").html(items);

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
                                    items += "<option value='" + location.postalCode + "'>" + location.placeName + " - " + location.postalCode  + "</option>";
                                });
                                $("#postal-code-dropdown").html(items);
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
    });

});