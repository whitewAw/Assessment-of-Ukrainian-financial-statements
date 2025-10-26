// Chrome Built-in AI API JavaScript Interop
// Provides access to window.ai.languageModel API for Blazor

let session = null;

// Check if Chrome AI is available
export async function checkAvailability() {
    try {
        if (!window.ai || !window.ai.languageModel) {
            return { available: false, reason: "Chrome AI API not found. Please use Chrome 127+ or Canary." };
        }

        const capabilities = await window.ai.languageModel.capabilities();

        if (capabilities.available === "readily") {
            return { available: true, reason: "Chrome AI is ready to use" };
        } else if (capabilities.available === "after-download") {
            return { available: false, reason: "Model needs to be downloaded. Visit chrome://components/ and update 'Optimization Guide On Device Model'" };
        } else {
            return { available: false, reason: `Chrome AI is not available: ${capabilities.available}` };
        }
    } catch (error) {
        console.error("Chrome AI availability check error:", error);
        return { available: false, reason: `Error: ${error.message}` };
    }
}

// Create a new AI session
export async function createSession() {
    try {
        if (!window.ai || !window.ai.languageModel) {
            throw new Error("Chrome AI API not available");
        }

        // Destroy existing session if any
        if (session) {
            try {
                await session.destroy();
            } catch (e) {
                console.warn("Error destroying previous session:", e);
            }
        }

        // Create new session with optimal parameters
        session = await window.ai.languageModel.create({
            temperature: 0.7,
            topK: 3,
        });

        console.log("Chrome AI session created successfully");
        return { success: true, message: "Session created" };
    } catch (error) {
        console.error("Chrome AI session creation error:", error);
        return { success: false, message: error.message };
    }
}

// Send a prompt to the AI
export async function prompt(message) {
    try {
        if (!session) {
            const sessionResult = await createSession();
            if (!sessionResult.success) {
                throw new Error("Failed to create session: " + sessionResult.message);
            }
        }

        const response = await session.prompt(message);
        return { success: true, response: response };
    } catch (error) {
        console.error("Chrome AI prompt error:", error);
        return { success: false, response: null, error: error.message };
    }
}

// Send a prompt with streaming response
export async function promptStreaming(message, dotnetHelper, callbackMethodName) {
    try {
        if (!session) {
            const sessionResult = await createSession();
            if (!sessionResult.success) {
                throw new Error("Failed to create session: " + sessionResult.message);
            }
        }

        const stream = await session.promptStreaming(message);
        let fullResponse = "";
        let previousLength = 0;

        for await (const chunk of stream) {
            fullResponse = chunk;
            const newContent = fullResponse.substring(previousLength);
            previousLength = fullResponse.length;

            // Call back to Blazor with the new chunk
            if (newContent && dotnetHelper) {
                await dotnetHelper.invokeMethodAsync(callbackMethodName, newContent);
            }
        }

        return { success: true, response: fullResponse };
    } catch (error) {
        console.error("Chrome AI streaming error:", error);
        return { success: false, response: null, error: error.message };
    }
}

// Destroy the session
export async function destroySession() {
    try {
        if (session) {
            await session.destroy();
            session = null;
            console.log("Chrome AI session destroyed");
        }
        return { success: true };
    } catch (error) {
        console.error("Chrome AI session destruction error:", error);
        return { success: false, message: error.message };
    }
}

// Clone the session (for parallel processing)
export async function cloneSession() {
    try {
        if (!session) {
            throw new Error("No active session to clone");
        }
        const clonedSession = await session.clone();
        return { success: true, session: clonedSession };
    } catch (error) {
        console.error("Chrome AI session cloning error:", error);
        return { success: false, message: error.message };
    }
}

// Get current token count
export async function getTokenCount() {
    try {
        if (!session) {
            return { success: false, count: 0, message: "No active session" };
        }

        // Note: Token counting API may vary based on Chrome version
        const tokensSoFar = await session.tokensSoFar || 0;
        const tokensLeft = await session.tokensLeft || 0;
        const maxTokens = await session.maxTokens || 0;

        return {
            success: true,
            tokensSoFar: tokensSoFar,
            tokensLeft: tokensLeft,
            maxTokens: maxTokens
        };
    } catch (error) {
        console.error("Chrome AI token count error:", error);
        return { success: false, message: error.message };
    }
}

// Initialize on load
console.log("Chrome AI JavaScript module loaded");
