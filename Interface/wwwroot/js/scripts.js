redirectToCheckout = function (sessionId) {
    var stripe = Stripe('pk_test_51It7o3EkGORhUfCrkwOM8jhCs6FFze8XB5Erhw0nHahxXABrbeZ5WPw93WewunrnDhVbBywR675tJzxPzaNxDLDg0055DbYjyM');
    stripe.redirectToCheckout({
        sessionId: sessionId
    });
};