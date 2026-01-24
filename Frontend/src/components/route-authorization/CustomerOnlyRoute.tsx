import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { PAGES } from '../../config/page';
import { USER_ROLES } from '../../types/common/enums/userRoles';
import { Loader } from 'lucide-react';

export default function CustomerOnlyRoute() {
    const { user, isLoading } = useAuth();

    if (isLoading) {
        return (
            <div className="flex justify-center items-center min-h-screen">
                <Loader className="w-8 h-8 text-brand-600 animate-spin" />
            </div>
        );
    }

    // Check if user is authenticated and has the Customer role
    if (user && user.roles && user.roles.includes(USER_ROLES.CUSTOMER)) {
        return <Outlet />;
    }

    // Redirect to login if not authenticated
    if (!user) {
        return <Navigate to={PAGES.IDENTITY.LOGIN.PATH} replace />;
    }

    // Redirect to home if authenticated but not a customer (or unauthorized page)
    return <Navigate to={PAGES.HOME.PATH} replace />;
}
