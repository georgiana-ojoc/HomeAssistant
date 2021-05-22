redirectToCheckout = function (sessionId) {
    const stripe = Stripe('pk_test_51ItvtzLd8HHMWYVMhybPjwpiHbfKbYvYxKwIC9luHrAwVifuuETdOC14G87RQSm1SHUG9P0es47ylQJqmWzt40Si00aDxLZpVj');
    stripe.redirectToCheckout({
        sessionId: sessionId
    });
};