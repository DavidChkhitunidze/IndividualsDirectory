$(document).ready(() => {
    const datePickerSelector = $("#sandbox-container .input-group.date");
    if (datePickerSelector.length > 0) {
        datePickerSelector.datepicker({
            format: "mm/dd/yyyy"
        });
    }

    const citiesSelector = $("#cities");
    if (citiesSelector.length > 0) {
        citiesSelector.select2({
            placeholder: "Choose...",
            minimumInputLength: 2,
            ajax: {
                cache: true,
                delay: 250,
                url: "/api/data/cities",
                dataType: "json",
                data: params => {
                    const query = { name: params.term };
                    return query;
                },
                processResults: result => {
                    const data = [];
                    if (result.success) {
                        const resultData = result.model;
                        for (var i = 0; i < resultData.length; i++) {
                            data.push({
                                id: resultData[i].id,
                                text: resultData[i].name
                            });
                        }
                        return {
                            results: data
                        };
                    }
                }
            }
        });

        const selected = citiesSelector.find(":selected");
        const cityId = selected.val();
        if (cityId) {
            $.get("/api/data/city?id=" + cityId)
                .then(result => {
                    if (result.success) {
                        const data = result.model;
                        const option = new Option(data.name, data.id, true, true);
                        citiesSelector.append(option).trigger("change");
                        citiesSelector.trigger({
                            type: "select2:select",
                            params: {
                                data: data
                            }
                        });
                    }
                });
        }
    }

    const addCreatePhoneNumberSelector = $("#addCreatePhoneNumber");
    if (addCreatePhoneNumberSelector.length > 0) {
        addCreatePhoneNumberSelector.click(() => {
            $.get("/Individuals/CreatePhoneNumbersEntryRaw", template => {
                $("#createPhoneNumbersEditor").append(template);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
            });
        });
    }

    const addCreateRelatedIndividualSelector = $("#addCreateRelatedIndividual");
    if (addCreateRelatedIndividualSelector.length > 0) {
        addCreateRelatedIndividualSelector.click(() => {
            $.get("/Individuals/CreateRelatedIndividualsEntryRaw", template => {
                $("#createRelatedIndividualsEditor").append(template);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                relatedIndividualsSelect2();
            });
        });
    }

    const addEditPhoneNumberSelector = $("#addEditPhoneNumber");
    if (addEditPhoneNumberSelector.length > 0) {
        addEditPhoneNumberSelector.click(() => {
            $.get("/Individuals/EditPhoneNumbersEntryRaw", template => {
                $("#editPhoneNumbersEditor").append(template);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
            });
        });
    }

    const addEditRelatedIndividualSelector = $("#addEditRelatedIndividual");
    if (addEditRelatedIndividualSelector.length > 0) {
        addEditRelatedIndividualSelector.click(() => {
            $.get("/Individuals/EditRelatedIndividualsEntryRaw", template => {
                $("#editRelatedIndividualsEditor").append(template);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                relatedIndividualsSelect2();
            });
        });
    }

    relatedIndividualsSelect2();
});

const relatedIndividualsSelect2 = () => {
    const relatedIndividualsSelector = $(".relatedIndividuals");
    if (relatedIndividualsSelector.length > 0) {
        relatedIndividualsSelector.select2({
            placeholder: "First Name | Last Name | Personal Number",
            minimumInputLength: 2,
            ajax: {
                cache: true,
                delay: 250,
                url: "/api/data/individuals",
                dataType: "json",
                data: params => {
                    const query = { search: params.term };
                    return query;
                },
                processResults: result => {
                    const data = [];
                    if (result.success) {
                        const resultData = result.model;
                        for (var i = 0; i < resultData.length; i++) {
                            data.push({
                                id: resultData[i].id,
                                text: `${resultData[i].firstName} ${resultData[i].lastName} | ${resultData[i].personalNumber}`
                            });
                        }
                        return {
                            results: data
                        };
                    }
                }
            }
        });

        for (var i = 0; i < relatedIndividualsSelector.length; i++) {
            const selector = relatedIndividualsSelector[i];
            const individualId = selector.firstElementChild.value;
            if (individualId) {
                $.get("/api/data/individual?id=" + individualId)
                    .then(result => {
                        if (result.success) {
                            const data = result.model;
                            const option = new Option(`${data.firstName} ${data.lastName} | ${data.personalNumber}`, data.id, true, true);
                            selector.append(option).trigger("change");
                            selector.trigger({
                                type: "select2:select",
                                params: {
                                    data: data
                                }
                            });
                        }
                    });
            }
        }
    }
};
